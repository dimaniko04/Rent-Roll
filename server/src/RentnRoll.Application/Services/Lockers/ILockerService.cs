using RentnRoll.Application.Contracts.Lockers.ConfigureCells;
using RentnRoll.Application.Contracts.Lockers.CreateLocker;
using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Contracts.Lockers.Response;
using RentnRoll.Application.Contracts.Lockers.UpdateLocker;
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
    Task<Result<LockerDetailsResponse>> UpdateLockerAsync(
        Guid lockerId,
        UpdateLockerRequest request);
    Task<Result<LockerResponse>> DeactivateLockerAsync(
        Guid lockerId);
    Task<Result<LockerResponse>> ActivateLockerAsync(
        Guid lockerId);
    Task<Result> DeleteLockerAsync(Guid lockerId);
    Task<Result<ICollection<CellResponse>>> ConfigureCellsAsync(
        Guid lockerId,
        ConfigureCellsRequest request);
    Task<Result> DeleteConfigurationAsync(
        string deviceId);
}