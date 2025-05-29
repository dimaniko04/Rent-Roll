
using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Categories;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class CategoryRepository
    : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}