using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Specifications.Lockers;

public sealed class GetAllLockersRequestSpec : Specification<Locker>
{
    public GetAllLockersRequestSpec(GetAllLockersRequest request)
    {
        IgnoreQueryFilters();
        AddCriteria(l => l.IsDeleted == request.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Country))
        {
            AddCriteria(l => l.Address.Country == request.Country);
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            AddCriteria(l => l.Address.City == request.City);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            AddCriteria(l => l.Name.Contains(request.Search));
        }

        AddInclude(l => l.Address);

        ApplyOrderBy(l => l.Name);
        ApplySorting(request.SortBy);
    }
}