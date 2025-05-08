using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Persistence.Identity.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        ILogger<AuthService> logger,
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _logger = logger;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponse>> LoginAsync(UserLoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.LogError(
                "User with email {Email} not found",
                request.Email
            );
            return Error.NotFound(
                message: "User with this email does not exist");
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            _logger.LogError(
                "Invalid password for user with email {Email}",
                request.Email
            );
            return Error.InvalidRequest(message: "Invalid email or password");
        }
        var roles = await _userManager.GetRolesAsync(user);

        var userResponse = new UserResponse(
            user.Id,
            user.Email!,
            user.Country,
            user.BirthDate,
            roles
        );

        var token = _jwtTokenGenerator.GenerateToken(userResponse);

        return new AuthResponse(
            token,
            userResponse
        );
    }

    public async Task<Result<AuthResponse>> RegisterAsync(
        UserRegisterRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Country = request.Country,
            BirthDate = request.BirthDate,
            PhoneNumber = request.PhoneNumber
        };

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

        var userResponse = new UserResponse(
            user.Id,
            user.Email,
            user.Country,
            user.BirthDate,
            new List<string> { "User" }
        );

        var token = _jwtTokenGenerator.GenerateToken(userResponse);

        return new AuthResponse(
            token,
            userResponse
        );
    }
}