using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}