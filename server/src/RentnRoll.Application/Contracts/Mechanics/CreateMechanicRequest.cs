using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Contracts.Mechanics;

public record CreateMechanicRequest(string Name)
{
    public Mechanic ToMechanic()
    {
        return new Mechanic
        {
            Id = Guid.NewGuid(),
            Name = Name
        };
    }
};