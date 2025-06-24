using RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;
using RentnRoll.Application.Contracts.PricingPolicies.GetAllPricingPolicies;
using RentnRoll.Application.Contracts.PricingPolicies.Response;
using RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.PricingPolicies;

public interface IPricingPolicyService
{
    Task<List<PricingPolicyResponse>> GetAllPricingPoliciesAsync(
        Guid businessId,
        GetAllPricingPoliciesRequest request);

    Task<Result<PricingPolicyDetailsResponse>> GetPricingPolicyAsync(
        Guid businessId,
        Guid pricingPolicyId);

    Task<Result<PricingPolicyDetailsResponse>> CreatePricingPolicyAsync(
        Guid businessId,
        CreatePricingPolicyRequest request);

    Task<Result<PricingPolicyResponse>> UpdatePricingPolicyAsync(
        Guid businessId,
        Guid pricingPolicyId,
        UpdatePricingPolicyRequest request);

    Task<Result> DeletePricingPolicyAsync(
        Guid businessId,
        Guid pricingPolicyId);
}