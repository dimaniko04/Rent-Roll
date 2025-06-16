using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Problem([Errors.User.Unauthorized]);
        }

        var result = await _userService.GetCurrentUserAsync(userId);

        return result.Match(Ok, Problem);
    }

    [HttpGet("me/rentals")]
    public async Task<IActionResult> GetCurrentUserRentals()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Problem([Errors.User.Unauthorized]);
        }

        var result = await _userService
            .GetCurrentUserRentalsAsync(userId);

        return result.Match(Ok, Problem);

    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser(
        UpdateUserRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Problem([Errors.User.Unauthorized]);
        }

        var result = await _userService
            .UpdateCurrentUserAsync(userId, request);

        return result.Match(Ok, Problem);
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] GetAllUsersRequest request)
    {
        var users = await _userService.GetAllUsersAsync(request);

        return Ok(users);
    }

    [HttpGet("{id}", Name = nameof(GetUserById))]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await _userService.GetUserById(id);

        return result.Match(Ok, Problem);
    }

    [HttpPost("admin")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateAdminUser(
        UserRegisterRequest request)
    {
        var result = await _userService.CreateAdminUserAsync(request);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return CreatedAtRoute(
            nameof(GetUserById),
            new { id = result.Value!.Id },
            result.Value
        );
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> BlockUser(string id)
    {
        var result = await _userService.BlockUserAsync(id);

        return result.Match(NoContent, Problem);
    }

    [HttpPut("{id}/restore")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RestoreUser(string id)
    {
        var result = await _userService.RestoreUserAsync(id);

        return result.Match(NoContent, Problem);
    }
}