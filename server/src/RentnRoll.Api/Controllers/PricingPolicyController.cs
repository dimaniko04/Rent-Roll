using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;
using RentnRoll.Application.Contracts.PricingPolicies.GetAllPricingPolicies;
using RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;
using RentnRoll.Application.Services.PricingPolicies;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/businesses/{businessId:guid}/policies")]
[Authorize]
public class PricingPolicyController : ApiController
{
    private readonly IPricingPolicyService _pricingPolicyService;

    public PricingPolicyController(
        IPricingPolicyService pricingPolicyService)
    {
        _pricingPolicyService = pricingPolicyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPolicies(
        Guid businessId,
        [FromQuery] GetAllPricingPoliciesRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var pricingPolicies = await _pricingPolicyService
            .GetAllPricingPoliciesAsync(businessId, request);

        return Ok(pricingPolicies);
    }

    [HttpGet("{policyId:guid}", Name = nameof(GetPolicyById))]
    public async Task<IActionResult> GetPolicyById(
        Guid businessId,
        Guid policyId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var pricingPolicy = await _pricingPolicyService
            .GetPricingPolicyAsync(businessId, policyId);

        return pricingPolicy.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePolicy(
        Guid businessId,
        [FromBody] CreatePricingPolicyRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _pricingPolicyService
            .CreatePricingPolicyAsync(businessId, request);

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtRoute(
            nameof(GetPolicyById),
            new { businessId, policyId = result.Value!.Id },
            result.Value);
    }

    [HttpPut("{policyId:guid}")]
    public async Task<IActionResult> UpdatePolicy(
        Guid businessId,
        Guid policyId,
        [FromBody] UpdatePricingPolicyRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _pricingPolicyService
            .UpdatePricingPolicyAsync(businessId, policyId, request);

        return result.Match(Ok, Problem);
    }

    [HttpDelete("{policyId:guid}")]
    public async Task<IActionResult> DeletePolicy(
        Guid businessId,
        Guid policyId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _pricingPolicyService
            .DeletePricingPolicyAsync(businessId, policyId);

        return result.Match(Ok, Problem);
    }
}