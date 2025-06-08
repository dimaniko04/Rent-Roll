using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;

public record CreatePricingPolicyRequest(
    string Name,
    string Description,
    string TimeUnit,
    int UnitCount,
    int PricePercent
)
{
    public PricingPolicy ToPricingPolicy(Guid businessId)
    {
        return new PricingPolicy
        {
            Name = Name,
            Description = Description,
            BusinessId = businessId,
            TimeUnit = Enum.Parse<TimeUnit>(TimeUnit, true),
            UnitCount = UnitCount,
            PricePercent = PricePercent,
        };
    }
};