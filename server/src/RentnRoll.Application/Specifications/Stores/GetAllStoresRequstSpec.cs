using RentnRoll.Application.Contracts.Stores.GetAllStores;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Specifications.Stores;

public sealed class GetAllStoresRequestSpec : Specification<Store>
{
    public GetAllStoresRequestSpec(
        Guid businessId,
        GetAllStoresRequest request)
    {
        AddCriteria(store => store.BusinessId == businessId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            AddCriteria(store => store.Name.Contains(request.Search));
        }

        AddInclude(store => store.Address);

        ApplyOrderBy(store => store.Name);
        ApplySorting(request.SortBy);
    }
}