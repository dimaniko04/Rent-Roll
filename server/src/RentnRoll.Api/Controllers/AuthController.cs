using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Authentication;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthController : ApiController
{
    private readonly IAuthService _userService;

    public AuthController(IAuthService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request);

        return result.Match(Ok, Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var result = await _userService.LoginAsync(request);

        return result.Match(Ok, Problem);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var tokenFromHeader = Request.Headers["x-refresh-token"].FirstOrDefault();
        var tokenFromCookie = Request.Cookies["refreshToken"];

        var refreshToken = tokenFromCookie ?? tokenFromHeader;

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Problem([Errors.Authentication.NoRefreshToken]);
        }

        var result = await _userService.RefreshTokenAsync(refreshToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Problem([Errors.Authentication.InvalidToken]);
        }

        var result = await _userService.LogoutAsync(userId);

        return result.Match(Ok, Problem);
    }
}