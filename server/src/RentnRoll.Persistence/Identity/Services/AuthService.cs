using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Persistence.Identity.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IValidationService _validationService;

    public AuthService(
        ITokenService tokenService,
        ILogger<AuthService> logger,
        UserManager<User> userManager,
        IValidationService validationService)
    {
        _logger = logger;
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

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            _logger.LogError(
                "User with email {Email} not found",
                request.Email
            );
            return Errors.Authentication.UserNotFound;
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
        var userResponse = user.ToUserResponse(roles);

        var (accessToken, refreshToken) = _tokenService
            .GenerateTokens(userResponse);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken
        );
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
            return Errors.Authentication.UserAlreadyExists;
        }

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

        var userResponse = user.ToUserResponse([Roles.User]);

        var (accessToken, refreshToken) = _tokenService
            .GenerateTokens(userResponse);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken
        );
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(
        RefreshRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var principal = _tokenService
            .GetTokenPrincipal(request.AccessToken)
            .Value;
        var userId = principal
            ?.FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;
        var user = await _userManager
            .FindByIdAsync(userId ?? string.Empty);

        if (user == null)
        {
            _logger.LogError("Invalid access token");
            return Errors.Authentication.InvalidToken;
        }

        if (user.RefreshToken != request.RefreshToken
            || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            _logger.LogError(
                "Invalid refresh token for user with email {Email}",
                user.Email
            );
            return Errors.Authentication.InvalidRefreshToken;
        }

        var userResponse = user.ToUserResponse([Roles.User]);
        var (accessToken, refreshToken) = _tokenService
            .GenerateTokens(userResponse);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken
        );
    }

    public async Task<Result> LogoutAsync(
        string userId)
    {
        var user = await _userManager
            .Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogError(
                "User with id {UserId} not found",
                userId
            );
            return Result.Failure([
                Errors.Authentication.UserNotFound
            ]);
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}