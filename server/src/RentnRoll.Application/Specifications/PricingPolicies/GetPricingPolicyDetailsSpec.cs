using RentnRoll.Application.Contracts.PricingPolicies.GetAllPricingPolicies;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.PricingPolicies;

namespace RentnRoll.Application.Specifications.PricingPolicies;

public sealed class GetPricingPolicyDetailsSpec
    : Specification<PricingPolicy>
{
    public GetPricingPolicyDetailsSpec(
        Guid businessId, Guid policyId)
    {
        AddCriteria(policy => policy.BusinessId == businessId);
        AddCriteria(policy => policy.Id == policyId);

        AddInclude(policy => policy.Items);
        AddInclude("Items.Game");
        AddInclude("Items.Game.Game");
        AddInclude("Items.Game.Tags");
    }
}