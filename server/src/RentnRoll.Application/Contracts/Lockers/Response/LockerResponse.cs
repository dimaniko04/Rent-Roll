using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Contracts.Lockers.Response;

public record LockerResponse(
    Guid Id,
    string Name,
    Address Address,
    bool IsActive
)
{
    public static LockerResponse FromLocker(Locker locker)
    {
        return new LockerResponse(
            locker.Id,
            locker.Name,
            locker.Address,
            locker.IsActive
        );
    }
}