using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Services.Rentals;

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

    [HttpGet]
    public async Task<IActionResult> GetAllRentals(
        [FromQuery] GetAllRentalsRequest request)
    {
        var rentals = await _rentalService
            .GetAllRentalsAsync(request);
        return Ok(rentals);
    }
}