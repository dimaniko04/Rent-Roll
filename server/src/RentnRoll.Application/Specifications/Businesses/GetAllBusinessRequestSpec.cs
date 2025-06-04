using RentnRoll.Application.Contracts.Businesses.GetAllBusinesses;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Specifications.Businesses;

public sealed class GetAllBusinessRequestSpec
    : Specification<Business>
{
    public GetAllBusinessRequestSpec(GetAllBusinessesRequest request)
    {
        IgnoreQueryFilters();
        AddCriteria(u => u.IsDeleted == request.IsDeleted);

        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(g => g.Name.Contains(request.Search));
        }

        ApplyOrderByDescending(u => u.Name);
        ApplySorting(request.SortBy);
        ApplyPaging(request.PageNumber, request.PageSize);
    }
}