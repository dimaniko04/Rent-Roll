using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Specifications.Stores;

public sealed class StoreNameSpec : Specification<Store>
{
    public StoreNameSpec(Guid businessId, string name)
    {
        AddCriteria(store => store.BusinessId == businessId);
        AddCriteria(store => store.Name == name);
    }
}