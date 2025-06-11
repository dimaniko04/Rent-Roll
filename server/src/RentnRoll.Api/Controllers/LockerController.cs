using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Lockers.CreateLocker;
using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Services.Lockers;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/lockers")]
[Authorize]
public class LockerController : ApiController
{
    private readonly ILockerService _lockerService;

    public LockerController(ILockerService lockerService)
    {
        _lockerService = lockerService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllLockers(
        [FromQuery] GetAllLockersRequest request)
    {
        var result = await _lockerService
            .GetAllLockersAsync(request);
        return Ok(result);
    }

    [HttpGet("{lockerId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetLockerById(Guid lockerId)
    {
        var result = await _lockerService
            .GetLockerByIdAsync(lockerId);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateLocker(
        [FromBody] CreateLockerRequest request)
    {
        var result = await _lockerService
            .CreateLockerAsync(request);
        return result.Match(Ok, Problem);
    }
}