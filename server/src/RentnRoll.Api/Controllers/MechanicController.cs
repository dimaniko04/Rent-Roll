using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Application.Services.Mechanics;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/mechanics")]
[Authorize]
public class MechanicController : ApiController
{
    private readonly IMechanicService _mechanicService;

    public MechanicController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMechanics(
        [FromQuery] GetAllMechanicsRequest request)
    {
        var result = await _mechanicService
            .GetAllMechanicsAsync(request);
        return Ok(result);
    }

    [HttpGet("{mechanicId:guid}")]
    public async Task<IActionResult> GetMechanicById(
        Guid mechanicId)
    {
        var result = await _mechanicService
            .GetMechanicByIdAsync(mechanicId);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateMechanic(
        CreateMechanicRequest request)
    {
        var result = await _mechanicService
            .CreateMechanicAsync(request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{mechanicId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateMechanic(
        Guid mechanicId,
        UpdateMechanicRequest request)
    {
        var result = await _mechanicService
            .UpdateMechanicAsync(mechanicId, request);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{mechanicId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteMechanic(
        Guid mechanicId)
    {
        var result = await _mechanicService
            .DeleteMechanicAsync(mechanicId);
        return result.Match(Ok, Problem);
    }
}