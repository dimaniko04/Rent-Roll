using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Specifications.Stores;

public sealed class GetStoreDetailsSpec : Specification<Store>
{
    public GetStoreDetailsSpec(
        Guid businessId,
        Guid storeId)
    {
        AddCriteria(store => store.BusinessId == businessId);
        AddCriteria(store => store.Id == storeId);

        AddInclude(store => store.Address);
        AddInclude(store => store.Assets);

        AddInclude("Policy");
        AddInclude("Assets.BusinessGame");
        AddInclude("Assets.BusinessGame.Game");
    }
}