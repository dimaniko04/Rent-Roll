using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Common;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Extensions;
using RentnRoll.Persistence.Specifications;

namespace RentnRoll.Persistence.Repositories;

public abstract class BaseRepository<TEntity>
    : IBaseRepository<TEntity> where TEntity : Entity
{
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly RentnRollDbContext _context;

    protected BaseRepository(RentnRollDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async virtual Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity>? specification,
        bool trackChanges = false)
    {
        var query = _dbSet.AsQueryable();

        if (specification != null)
        {
            query = SpecificationEvaluator.GetQuery(query, specification);
        }

        return trackChanges ?
            await query.ToListAsync() :
            await query.AsNoTracking().ToListAsync();
    }

    public async virtual Task<PaginatedResponse<TEntity>> GetPaginatedAsync(
        ISpecification<TEntity> specification,
        bool trackChanges = false)
    {
        if (!specification.IsPagingEnabled)
        {
            throw new ArgumentException("Paging is not enabled for this specification.");
        }

        var pageNumber = specification.PageNumber;
        var pageSize = specification.PageSize;
        var query = _dbSet.AsQueryable();

        if (specification != null)
        {
            query = SpecificationEvaluator.GetQuery(query, specification);
        }

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        return await query.ToPaginatedResponse(pageNumber, pageSize);
    }

    public async virtual Task<TEntity?> GetSingleAsync(
        ISpecification<TEntity> specification,
        bool trackChanges = false)
    {
        var query = _dbSet.AsQueryable();

        query = SpecificationEvaluator.GetQuery(query, specification);

        return trackChanges ?
            await query.FirstOrDefaultAsync() :
            await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async virtual Task<TEntity?> GetByIdAsync(
        Guid id,
        bool trackChanges = false)
    {
        return trackChanges ?
            await _dbSet.FindAsync(id) :
            await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public async virtual Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}