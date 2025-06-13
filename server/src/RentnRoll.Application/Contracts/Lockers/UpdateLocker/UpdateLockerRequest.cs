using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Contracts.Lockers.UpdateLocker;

public record UpdateLockerRequest(
    string Name,
    Address Address,
    int NumberOfCells
);