using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IMechanicRepository : IBaseRepository<Mechanic>
{
    Task<Mechanic?> GetByNameAsync(string name);
}