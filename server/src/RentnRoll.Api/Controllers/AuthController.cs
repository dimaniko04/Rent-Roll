using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Authentication;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api")]
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
}