using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface ILockerRepository : IBaseRepository<Locker>
{
    public Task<Locker?>
        GetByIdWithUnconfiguredCellsAsync(Guid lockerId);
    public Task<ICollection<Cell>>
        GetCellsByIotDeviceIdAsync(string iotDeviceId);
    public Task<Locker?>
        GetByIotDeviceIdAsync(string iotDeviceId);
}