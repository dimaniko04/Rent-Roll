using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Rentals.CreateRental;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Services.Rentals;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/rentals")]
[Authorize]
public class RentalController : ApiController
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpGet(Name = nameof(GetAllRentals))]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllRentals(
        [FromQuery] GetAllRentalsRequest request)
    {
        var rentals = await _rentalService
            .GetAllRentalsAsync(request);
        return Ok(rentals);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetUserRentals()
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var rentals = await _rentalService
            .GetAllUserRentalsAsync(userId);
        return Ok(rentals);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRental(
        [FromBody] CreateRentalRequest request)
    {
        var result = await _rentalService
            .CreateRentalAsync(request);

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtAction(
            nameof(GetAllRentals),
            new { id = request.Id });
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> CancelRental(Guid id)
    {
        var result = await _rentalService
            .CancelRentalAsync(id);

        return result.Match(NoContent, Problem);
    }

    [HttpPut("{id:guid}/pick")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> ConfirmPickUp(Guid id)
    {
        var result = await _rentalService
            .ConfirmStorePickUpAsync(id);

        return result.Match(NoContent, Problem);
    }

    [HttpPut("{id:guid}/return")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> ConfirmReturn(Guid id)
    {
        var result = await _rentalService
            .ConfirmStoreReturnAsync(id);

        return result.Match(NoContent, Problem);
    }

    [HttpPut("{id:guid}/open/{openReason}")]
    public async Task<IActionResult> OpenCell(
        Guid id,
        string openReason)
    {
        var result = await _rentalService
            .OpenCellAsync(id, openReason);

        return result.Match(NoContent, Problem);
    }

    [HttpPut("{id:guid}/solve/{solution}")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> SolveMaintenance(
        Guid id,
        string solution)
    {
        var result = await _rentalService
            .SolveMaintenance(id, solution);

        return result.Match(NoContent, Problem);
    }
}