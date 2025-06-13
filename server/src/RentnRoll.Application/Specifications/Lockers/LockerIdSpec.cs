using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Specifications.Lockers;

public sealed class LockerIdSpec : Specification<Locker>
{
    public LockerIdSpec(Guid lockerId)
    {
        IgnoreQueryFilters();
        AddCriteria(l => l.Id == lockerId);
    }
}