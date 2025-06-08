using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Domain.Entities.PricingPolicies;

namespace RentnRoll.Application.Contracts.PricingPolicies.Response;

public record PricingPolicyItemResponse(
    int Price,
    BusinessGameResponse Game)
{
    public static PricingPolicyItemResponse FromPricingPolicyItem(
        PricingPolicyItem item)
    {
        return new PricingPolicyItemResponse(
            item.Price,
            BusinessGameResponse
                .FromBusinessGame(item.Game)
        );
    }
}