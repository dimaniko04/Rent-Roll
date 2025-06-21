using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Specifications.Lockers;

public sealed class GetAllBusinessLockersRequestSpec
    : Specification<Locker>
{
    public GetAllBusinessLockersRequestSpec(
        Guid businessId,
        GetAllLockersRequest request)
    {
        AddCriteria(l => l.IsActive == request.IsActive);
        AddCriteria(l => l.Cells.Any(c => c.BusinessId == businessId));

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

        AddInclude(l => l.Cells.Where(c => c.BusinessId == businessId));
        AddInclude(l => l.PricingPolicies
            .Where(p => p.BusinessId == businessId));

        AddInclude("Cells.Business");
        AddInclude("Cells.BusinessGame");
        AddInclude("Cells.BusinessGame.Game");

        ApplyOrderBy(l => l.Name);
        ApplySorting(request.SortBy);
    }
}