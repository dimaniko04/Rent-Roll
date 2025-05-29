using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Extensions;
using RentnRoll.Persistence.Specifications;

namespace RentnRoll.Persistence.Identity.Services;

public class UserService : IUserService
{
    private readonly RentnRollDbContext _dbContext;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IValidationService _validationService;

    public UserService(
        ILogger<UserService> logger,
        RentnRollDbContext dbContext,
        UserManager<User> userManager,
        IValidationService validationService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
        _validationService = validationService;
    }

    public async Task<Result> BlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} not found or blocked",
                userId
            );
            return Result.Failure([Errors.User.NotFound]);
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

        if (isAdmin)
        {
            _logger.LogError(
                "User with id {UserId} is an admin and cannot be blocked",
                userId
            );
            return Result.Failure([Errors.User.AdminCannotBeBlocked]);
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to block user with id {UserId}: {Errors}",
                userId,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return Result.Failure(result.Errors
                .Select(e => Error.InvalidRequest(e.Code, e.Description))
                .ToList());
        }

        return Result.Success();
    }

    public async Task<Result> RestoreUserAsync(string userId)
    {
        _logger.LogInformation(
            "Restoring user with id {UserId}",
            userId
        );

        var user = await _userManager
            .Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} does not exist",
                userId
            );
            return Result.Failure([Errors.User.NotFound]);
        }

        user.DeletedAt = null;
        user.IsDeleted = false;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to restore user with id {UserId}: {Errors}",
                userId,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return Result.Failure(result.Errors
                .Select(e => Error.InvalidRequest(e.Code, e.Description))
                .ToList());
        }


        return Result.Success();
    }

    public async Task<Result<DetailedUserResponse>> CreateAdminUserAsync(
        UserRegisterRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var exists = await _userManager.FindByEmailAsync(request.Email);

        if (exists != null)
        {
            _logger.LogError(
                "User with email {Email} already exists",
                request.Email
            );
            return Errors.User.AlreadyExists;
        }

        var user = User.FromUserRegisterRequest(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Validation errors occurred while creating admin user with email {Email}: {Errors}",
                request.Email,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        result = await _userManager.AddToRoleAsync(user, Roles.Admin);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to add user with email {Email} to role {Role}: {Errors}",
                request.Email,
                Roles.Admin,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return result.Errors
                .Select(e => Error.InvalidRequest(e.Code, e.Description))
                .ToList();
        }

        var userResponse = user.ToDetailedUserResponse([Roles.Admin]);

        return userResponse;
    }

    public async Task<Result<DetailedUserResponse>> PromoteUserToAdminAsync(
        string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} not found or blocked",
                userId
            );
            return Errors.User.NotFound;
        }

        var result = await _userManager.AddToRoleAsync(user, Roles.Admin);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to promote user with id {UserId} to admin: {Errors}",
                userId,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }
        var roles = await _userManager.GetRolesAsync(user);
        var userResponse = user.ToDetailedUserResponse(roles);

        return userResponse;
    }

    public async Task<PaginatedResponse<UserResponse>> GetAllUsersAsync(
        GetAllUsersRequest request)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbContext.Users.IgnoreQueryFilters(),
            new GetAllUsersRequestSpec(request)
        );

        if (!string.IsNullOrEmpty(request.Role))
        {
            query = query.Join(
                _dbContext.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRole) => new { User = user, userRole.RoleId }
            ).Join(
                _dbContext.Roles,
                obj => obj.RoleId,
                role => role.Id,
                (obj, role) => new { obj.User, Role = role.Name }
            )
            .Where(u => u.Role == request.Role)
            .Select(u => u.User);
        }

        var users = await query
            .Select(u => u.ToUserResponse())
            .ToPaginatedResponse(
                request.PageNumber,
                request.PageSize
            );

        return users;
    }

    public async Task<Result<DetailedUserResponse>> GetUserById(
        string userId)
    {
        var user = await _userManager
            .Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} not found",
                userId
            );
            return Errors.User.NotFound;
        }
        var roles = await _userManager.GetRolesAsync(user);

        return user.ToDetailedUserResponse(roles);
    }

    public async Task<Result<DetailedUserResponse>> GetCurrentUserAsync(
        string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} is blocked",
                userId
            );
            return Errors.User.Blocked;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userResponse = user.ToDetailedUserResponse(roles);

        return userResponse;
    }

    public async Task<Result<DetailedUserResponse>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} is blocked",
                userId
            );
            return Errors.User.Blocked;
        }

        MapUpdateRequestToUser(user, request);
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to update user with id {UserId}: {Errors}",
                userId,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userResponse = user.ToDetailedUserResponse(roles);

        return userResponse;
    }

    private void MapUpdateRequestToUser(
        User user,
        UpdateUserRequest request)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.Country = request.Country;
        user.BirthDate = request.BirthDate;
        user.Email = request.Email;
        user.UserName = request.Email;
    }
}