using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Contracts.Mechanics;

public record MechanicResponse(Guid Id, string Name)
{
    public static MechanicResponse FromMechanic(Mechanic Mechanic)
    {
        return new MechanicResponse(Mechanic.Id, Mechanic.Name);
    }
}