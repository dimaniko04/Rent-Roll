using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Services.Mechanics;

public interface IMechanicService
{
    Task<Result<MechanicResponse>> CreateMechanicAsync(
        CreateMechanicRequest request);

    Task<Result<MechanicResponse>> UpdateMechanicAsync(
        Guid MechanicId,
        UpdateMechanicRequest request);

    Task<Result<MechanicResponse>> GetMechanicByIdAsync(
        Guid MechanicId);

    Task<IEnumerable<MechanicResponse>> GetAllMechanicsAsync(
        GetAllMechanicsRequest request);

    Task<Result> DeleteMechanicAsync(Guid MechanicId);
}