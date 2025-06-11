using RentnRoll.Application.Contracts.Lockers.CreateLocker;
using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Contracts.Lockers.Response;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Lockers;

public interface ILockerService
{
    Task<ICollection<LockerResponse>> GetAllLockersAsync(
        GetAllLockersRequest request);
    Task<Result<LockerDetailsResponse>> GetLockerByIdAsync(
        Guid lockerId);
    Task<Result<LockerResponse>> CreateLockerAsync(
        CreateLockerRequest request);
}