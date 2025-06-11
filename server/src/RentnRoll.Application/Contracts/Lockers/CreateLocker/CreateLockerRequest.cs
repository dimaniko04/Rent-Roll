using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Contracts.Lockers.CreateLocker;

public record CreateLockerRequest(
    string Name,
    int NumberOfCells,
    Address Address
)
{
    public Locker ToLocker()
    {
        return new Locker
        {
            Name = Name,
            Address = Address,
            Cells = [.. Enumerable
                .Range(1, NumberOfCells)
                .Select(i => new Cell())],
        };
    }
}