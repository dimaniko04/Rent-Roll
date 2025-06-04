using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Specifications.Businesses;

public sealed class GetBusinessByIdWithOwnerSpec
    : Specification<Business>
{
    public GetBusinessByIdWithOwnerSpec(Guid id)
    {
        IgnoreQueryFilters();
        AddCriteria(b => b.Id == id);
    }
}