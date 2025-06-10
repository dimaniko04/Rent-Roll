using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.Stores.CreateStore;
using RentnRoll.Application.Contracts.Stores.GetAllStores;
using RentnRoll.Application.Contracts.Stores.UpdateStore;
using RentnRoll.Application.Services.Stores;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/businesses/{businessId:guid}/stores")]
[Authorize]
public class StoreController : ApiController
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStores(
        Guid businessId,
        [FromQuery] GetAllStoresRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var stores = await _storeService
            .GetAllStoresAsync(businessId, request);

        return Ok(stores);
    }

    [HttpGet("{storeId:guid}", Name = nameof(GetStoreById))]
    public async Task<IActionResult> GetStoreById(
        Guid businessId,
        Guid storeId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _storeService
            .GetStoreByIdAsync(businessId, storeId);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStore(
        Guid businessId,
        [FromBody] CreateStoreRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _storeService
            .CreateStoreAsync(businessId, request);

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtRoute(
            nameof(GetStoreById),
            new { businessId, storeId = result.Value!.Id },
            result.Value);
    }

    [HttpPut("{storeId:guid}")]
    public async Task<IActionResult> UpdateStore(
        Guid businessId,
        Guid storeId,
        [FromBody] UpdateStoreRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _storeService
            .UpdateStoreAsync(businessId, storeId, request);

        return result.Match(Ok, Problem);
    }

    [HttpDelete("{storeId:guid}")]
    public async Task<IActionResult> DeleteStore(
        Guid businessId,
        Guid storeId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _storeService
            .DeleteStoreAsync(businessId, storeId);

        return result.Match(Ok, Problem);
    }
}