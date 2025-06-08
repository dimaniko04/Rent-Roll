using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.PricingPolicies;

namespace RentnRoll.Application.Specifications.PricingPolicies;

public sealed class PricingPolicyNameSpec
    : Specification<PricingPolicy>
{
    public PricingPolicyNameSpec(
        Guid businessId,
        string name)
    {
        AddCriteria(policy => policy.BusinessId == businessId);
        AddCriteria(policy => policy.Name == name);
    }
}