using RentnRoll.Domain.Entities.Genres;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IGenreRepository : IBaseRepository<Genre>
{
    Task<Genre?> GetByNameAsync(string name);
}