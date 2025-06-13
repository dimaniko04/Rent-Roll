using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Lockers.ConfigureCells;
using RentnRoll.Application.Contracts.Lockers.CreateLocker;
using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Contracts.Lockers.UpdateLocker;
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

    [HttpPut("{lockerId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateLocker(
        Guid lockerId,
        [FromBody] UpdateLockerRequest request)
    {
        var result = await _lockerService
            .UpdateLockerAsync(lockerId, request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{lockerId:guid}/deactivate")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeactivateLocker(Guid lockerId)
    {
        var result = await _lockerService
            .DeactivateLockerAsync(lockerId);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{lockerId:guid}/activate")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ActivateLocker(Guid lockerId)
    {
        var result = await _lockerService
            .ActivateLockerAsync(lockerId);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{lockerId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteLocker(Guid lockerId)
    {
        var result = await _lockerService
            .DeleteLockerAsync(lockerId);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{lockerId:guid}/cells/configure")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ConfigureCells(
        Guid lockerId,
        [FromBody] ConfigureCellsRequest request)
    {
        var result = await _lockerService
            .ConfigureCellsAsync(lockerId, request);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("cells/configure/{deviceId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveCellConfiguration(
        string deviceId)
    {
        var result = await _lockerService
            .DeleteConfigurationAsync(deviceId);
        return result.Match(Ok, Problem);
    }
}