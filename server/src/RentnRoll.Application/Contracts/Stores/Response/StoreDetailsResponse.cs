using RentnRoll.Application.Contracts.PricingPolicies.Response;
using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Contracts.Stores.Response;

public record StoreDetailsResponse(
    Guid Id,
    string Name,
    string Address,
    DateTime CreatedAt,
    PricingPolicyResponse? PricingPolicy,
    ICollection<StoreAssetResponse> Assets
)
{
    public static StoreDetailsResponse FromStore(Store store)
    {
        return new StoreDetailsResponse(
            store.Id,
            store.Name,
            store.Address.ToString(),
            store.CreatedAt,
            store.Policy is not null ?
                PricingPolicyResponse.FromPricingPolicy(store.Policy) :
                null,
            [.. store.Assets.Select(StoreAssetResponse.FromStoreAsset)]
        );
    }
}