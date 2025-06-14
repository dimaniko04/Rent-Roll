using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.Lockers.AssignBusiness;
using RentnRoll.Application.Contracts.Lockers.AssignGames;
using RentnRoll.Application.Contracts.Lockers.AssignPricingPolicy;
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

    [HttpGet("businesses/{businessId:guid}")]
    public async Task<IActionResult> AssignPolicyToLocker(
        Guid businessId,
        [FromQuery] GetAllLockersRequest request)
    {
        var authorizeResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);

        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var result = await _lockerService
            .GetAllBusinessLockersAsync(businessId, request);

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

    [HttpPut("{lockerId:guid}/cells/businesses/assign")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignCellsToBusiness(
        Guid lockerId,
        [FromBody] AssignBusinessRequest request)
    {
        var result = await _lockerService
            .AssignBusinessAsync(lockerId, request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{lockerId:guid}/{businessId:guid}/games/assign")]
    public async Task<IActionResult> AssignGamesToCells(
        Guid lockerId,
        Guid businessId,
        [FromBody] AssignGamesRequest request)
    {
        var ids = request
            .GameAssignments?
            .Select(a => a.CellId)
            .ToList() ?? [];

        var authorizeResult = await AuthorizeForResource(
            request,
            Policy.AssignedBusinessOwner);

        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var result = await _lockerService
            .AssignGamesAsync(lockerId, businessId, request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{lockerId:guid}/{businessId:guid}/policies")]
    public async Task<IActionResult> AssignPolicyToLocker(
        Guid lockerId,
        Guid businessId,
        [FromBody] AssignPricingPolicyRequest request)
    {
        var authorizeResult = await AuthorizeForResource(
            lockerId, Policy.HasCellAssignments);

        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var result = await _lockerService
            .AssignPricingPolicyAsync(lockerId, businessId, request);
        return result.Match(Ok, Problem);
    }
}