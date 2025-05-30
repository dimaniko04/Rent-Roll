using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IBaseRepository<TEntity> : IRepository
    where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity>? specification,
        bool trackChanges = false);
    Task<TEntity?> GetByIdAsync(Guid id, bool trackChanges = false);
    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}