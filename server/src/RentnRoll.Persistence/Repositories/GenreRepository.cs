
using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Genres;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class GenreRepository
    : BaseRepository<Genre>, IGenreRepository
{
    public GenreRepository(RentnRollDbContext context) : base(context)
    {
    }

    public async Task<Genre?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}