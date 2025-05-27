using Microsoft.EntityFrameworkCore;

using RentnRoll.Domain.Common;

namespace RentnRoll.Persistence.Specifications.Common;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
        where TEntity : class
    {
        var query = inputQuery;

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        specification.Includes.Aggregate(
            query,
            (current, include) => current.Include(include));

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsPagingEnabled)
        {
            var pageSize = specification.PageSize;
            var pageNumber = specification.PageNumber;

            query = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);
        }

        return query;
    }

}