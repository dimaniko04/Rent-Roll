using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Mechanics;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class MechanicRepository
    : BaseRepository<Mechanic>, IMechanicRepository
{
    public MechanicRepository(RentnRollDbContext context) : base(context)
    {
    }

    public async Task<Mechanic?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}