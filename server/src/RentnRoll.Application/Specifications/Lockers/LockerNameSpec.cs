using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Specifications.Lockers;

public sealed class LockerNameSpec : Specification<Locker>
{
    public LockerNameSpec(string name)
    {
        AddCriteria(l => l.Name == name);
    }
}