using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Specifications.Mechanics;

public sealed class MechanicNamesSpec : Specification<Mechanic>
{
    public MechanicNamesSpec(ICollection<string> names)
    {
        AddCriteria(m => names.Contains(m.Name));
    }
}