

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Businesses.CreateBusiness;
using RentnRoll.Application.Contracts.Businesses.GetAllBusinesses;
using RentnRoll.Application.Contracts.Businesses.UpdateBusiness;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Services.Businesses;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/businesses")]
[Authorize]
public class BusinessController : ApiController
{
    private readonly IBusinessService _businessService;

    public BusinessController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllBusinesses(
        [FromQuery] GetAllBusinessesRequest request)
    {
        var result = await _businessService
            .GetPaginatedAsync(request);

        return Ok(result);
    }

    [HttpGet("{businessId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetBusinessById(Guid businessId)
    {
        var result = await _businessService
            .GetByIdAsync(businessId);

        return result.Match(Ok, Problem);
    }

    [HttpDelete("{businessId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> BlockBusiness(Guid businessId)
    {
        var result = await _businessService
            .BlockAsync(businessId);

        return result.Match(Ok, Problem);
    }

    [HttpPut("{businessId:guid}/restore")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RestoreBusiness(Guid businessId)
    {
        var result = await _businessService
            .RestoreAsync(businessId);

        return result.Match(Ok, Problem);
    }

    [HttpGet("my")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> GetMyBusiness()
    {
        var result = await _businessService
            .GetMyBusinessAsync();

        return result.Match(Ok, Problem);
    }

    [HttpGet("my/rentals")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> GetMyBusinessRentals(
        [FromQuery] GetAllRentalsRequest request)
    {
        var result = await _businessService
            .GetMyRentalsAsync(request);

        return result.Match(Ok, Problem);
    }

    [HttpPost("my")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> CreateMyBusiness(
        CreateBusinessRequest request)
    {
        var result = await _businessService
            .CreateAsync(request);

        return result.Match(Ok, Problem);
    }

    [HttpPut("my")]
    [Authorize(Roles = Roles.Business)]
    public async Task<IActionResult> UpdateMyBusiness(
        UpdateBusinessRequest request)
    {
        var result = await _businessService
            .UpdateAsync(request);

        return result.Match(Ok, Problem);
    }
}