using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.PricingPolicies.Response;

public record PricingPolicyDetailsResponse(
    Guid Id,
    string Name,
    string? Description,
    TimeUnit TimeUnit,
    int UnitCount,
    int PricePercent,
    IList<PricingPolicyItemResponse> Items
)
{
    public static PricingPolicyDetailsResponse FromPricingPolicy(
        PricingPolicy policy)
    {
        return new PricingPolicyDetailsResponse(
            policy.Id,
            policy.Name,
            policy.Description,
            policy.TimeUnit,
            policy.UnitCount,
            policy.PricePercent,
            policy.Items
                .Select(PricingPolicyItemResponse
                    .FromPricingPolicyItem)
                .ToList()
        );
    }
}