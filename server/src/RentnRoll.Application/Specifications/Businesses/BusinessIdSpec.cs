using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Specifications.Businesses;

public sealed class BusinessIdSpec : Specification<Business>
{
    public BusinessIdSpec(Guid id)
    {
        IgnoreQueryFilters();
        AddCriteria(b => b.Id == id);
    }
}