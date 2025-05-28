using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Identity.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly RentnRollDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly IValidationService _validationService;

    public AuthService(
        ITokenService tokenService,
        ILogger<AuthService> logger,
        RentnRollDbContext dbContext,
        UserManager<User> userManager,
        IValidationService validationService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
        _tokenService = tokenService;
        _validationService = validationService;
    }

    public async Task<Result<AuthResponse>> LoginAsync(
        UserLoginRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var user = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            _logger.LogError(
                "User with email {Email} not found",
                request.Email
            );
            return Errors.User.NotFound;
        }
        if (user.IsDeleted)
        {
            _logger.LogError(
                "User with email {Email} is blocked",
                request.Email
            );
            return Errors.User.Blocked;
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            _logger.LogError(
                "Invalid password for user with email {Email}",
                request.Email
            );
            return Errors.Authentication.InvalidCredentials;
        }
        var roles = await _userManager.GetRolesAsync(user);

        return await GenerateAuthResponseAsync(user, roles);
    }

    public async Task<Result<AuthResponse>> RegisterAsync(
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
                "Validation errors occurred while creating user with email {Email}: {Errors}",
                request.Email,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        result = await _userManager.AddToRoleAsync(user, Roles.User);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to add user with email {Email} to role {Role}: {Errors}",
                request.Email,
                Roles.User,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return result.Errors
                .Select(e => Error.InvalidRequest(e.Code, e.Description))
                .ToList();
        }

        return await GenerateAuthResponseAsync(user, [Roles.User]);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(
        string refreshToken)
    {
        var user = await _dbContext
            .Set<User>()
            .FirstOrDefaultAsync(u =>
                u.RefreshToken == refreshToken &&
                u.RefreshTokenExpiry > DateTime.UtcNow);

        if (user == null)
        {
            _logger.LogError("Invalid access token");
            return Errors.Authentication.InvalidToken;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return await GenerateAuthResponseAsync(user, roles);
    }

    public async Task<Result> LogoutAsync(
        string userId)
    {
        var user = await _userManager
            .FindByIdAsync(userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} not found",
                userId
            );
            return Result.Failure([
                Errors.User.NotFound
            ]);
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await _userManager.UpdateAsync(user);
        _tokenService.SetTokenCookie(string.Empty, isExpired: true);

        return Result.Success();
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(
        User user,
        IEnumerable<string> roles)
    {
        var userResponse = user.ToDetailedUserResponse(roles);
        var (accessToken, refreshToken) = _tokenService
            .GenerateTokens(userResponse);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        _tokenService.SetTokenCookie(refreshToken, isExpired: false);

        return new AuthResponse(
            accessToken,
            refreshToken
        );
    }
}