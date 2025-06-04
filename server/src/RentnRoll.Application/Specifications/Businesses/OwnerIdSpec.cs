using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Specifications.Businesses;

public sealed class OwnerIdSpec : Specification<Business>
{
    public OwnerIdSpec(string id)
    {
        IgnoreQueryFilters();
        AddCriteria(b => b.OwnerId == id);
    }
}