using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Contracts.Stores.UpdateStore;

public record UpdateStoreRequest(
    string Name,
    Address Address,
    Guid? PolicyId,
    ICollection<UpdateStoreAssetRequest>? Assets
)
{
    public Store UpdateStore(
        Store store,
        PricingPolicy? policy)
    {
        store.Name = Name;
        store.Address = Address;
        store.Policy = policy;

        if (Assets != null)
        {
            store.Assets = Assets
                .Select(item => new StoreAsset
                {
                    Quantity = item.Quantity,
                    BusinessGameId = item.BusinessGameId
                })
                .ToList();
        }

        return store;
    }
};

