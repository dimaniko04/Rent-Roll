using RentnRoll.Core.Entities;

namespace RentnRoll.Application.Common.Interfaces.Persistence.Repositories;

public interface IBaseRepository<TEntity> : IRepository
    where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false);
    Task<TEntity?> GetByIdAsync(Guid id, bool trackChanges = false);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}