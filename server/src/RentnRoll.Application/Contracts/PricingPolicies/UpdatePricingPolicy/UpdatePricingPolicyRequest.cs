using RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;

public record UpdatePricingPolicyRequest(
    string Name,
    string Description,
    string TimeUnit,
    int UnitCount,
    int PricePercent,
    ICollection<UpdatePricingPolicyItemRequest>? Items
) : CreatePricingPolicyRequest(
    Name,
    Description,
    TimeUnit,
    UnitCount,
    PricePercent
)
{
    public PricingPolicy UpdatePricingPolicy(PricingPolicy policy)
    {
        policy.Name = Name;
        policy.Description = Description;
        policy.TimeUnit = Enum.Parse<TimeUnit>(TimeUnit, true);
        policy.UnitCount = UnitCount;
        policy.PricePercent = PricePercent;

        if (Items != null)
        {
            policy.Items = Items
                .Select(item => new PricingPolicyItem
                {
                    GameId = item.BusinessGameId,
                    Price = item.Price
                })
                .ToList();
        }

        return policy;
    }
}