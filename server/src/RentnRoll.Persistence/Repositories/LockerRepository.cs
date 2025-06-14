using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class LockerRepository
    : BaseRepository<Locker>, ILockerRepository
{
    public LockerRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public async Task<Locker?>
        GetByIdWithUnconfiguredCellsAsync(Guid lockerId)
    {
        return await _dbSet
            .Where(l => l.Id == lockerId)
            .Include(l => l.Cells.Where(c => c.IotDeviceId == null))
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Cell>>
        GetCellsByIotDeviceIdAsync(string iotDeviceId)
    {
        return await _dbSet
            .Include(l => l.Cells)
            .AsNoTracking()
            .SelectMany(l => l.Cells)
            .Where(c => c.IotDeviceId == iotDeviceId)
            .ToListAsync();
    }

    public async Task<Locker?>
        GetByIotDeviceIdAsync(string iotDeviceId)
    {
        return await _dbSet
            .Include(l => l.Cells.Where(c => c.IotDeviceId == iotDeviceId))
            .Where(l => l.Cells.Any(c => c.IotDeviceId == iotDeviceId))
            .FirstOrDefaultAsync();
    }

    public async Task<Locker?>
        GetConfiguredUnassigned(Guid lockerId)
    {
        return await _dbSet
            .Where(l => l.Id == lockerId)
            .Where(l => l.IsActive)
            .Include(l => l.Cells
                .Where(c => c.IotDeviceId != null)
                .Where(c => c.BusinessId == null))
            .FirstOrDefaultAsync();
    }

    public async Task<Locker?> GetCellsByIdsAsync(
        Guid lockerId,
        ICollection<Guid> cellIds)
    {
        return await _dbSet
            .Where(l => l.Id == lockerId)
            .Where(l => l.IsActive)
            .Include(l => l.Cells
                .Where(c => cellIds.Contains(c.Id)))
            .ThenInclude(c => c.Business)
            .FirstOrDefaultAsync();
    }
}