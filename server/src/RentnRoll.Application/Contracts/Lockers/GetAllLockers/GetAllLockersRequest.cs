using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Application.Contracts.Lockers.GetAllLockers;

public record GetAllLockersRequest(
    string City = "",
    string Country = "",
    bool IsDeleted = false
) : QueryParams;