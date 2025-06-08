using RentnRoll.Application.Contracts.PricingPolicies.GetAllPricingPolicies;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.PricingPolicies;

namespace RentnRoll.Application.Specifications.PricingPolicies;

public sealed class GetAllPricingPoliciesRequestSpec
    : Specification<PricingPolicy>
{
    public GetAllPricingPoliciesRequestSpec(
        Guid businessId,
        GetAllPricingPoliciesRequest request)
    {
        AddCriteria(policy => policy.BusinessId == businessId);

        if (request.TimeUnit.HasValue)
        {
            AddCriteria(policy => policy.TimeUnit == request.TimeUnit.Value);
        }

        if (request.EmptyOnly)
        {
            AddInclude(policy => policy.Items);
            AddCriteria(policy => !policy.Items.Any());
        }

        ApplyOrderBy(policy => policy.Name);
        ApplySorting(request.SortBy);
    }
}