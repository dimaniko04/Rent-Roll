using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.PricingPolicies.Response;

public record PricingPolicyResponse(
    Guid Id,
    string Name,
    TimeUnit TimeUnit,
    int UnitCount,
    int PricePercent
)
{
    public static PricingPolicyResponse FromPricingPolicy(
        PricingPolicy policy)
    {
        return new PricingPolicyResponse(
            policy.Id,
            policy.Name,
            policy.TimeUnit,
            policy.UnitCount,
            policy.PricePercent
        );
    }
}