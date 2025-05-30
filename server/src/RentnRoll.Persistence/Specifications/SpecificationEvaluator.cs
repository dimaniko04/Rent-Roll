using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

using RentnRoll.Application.Specifications.Common;

namespace RentnRoll.Persistence.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
        where TEntity : class
    {
        var query = inputQuery;

        if (specification.Criteria.Any())
        {
            foreach (var criteria in specification.Criteria)
            {
                query = query.Where(criteria);
            }
        }

        specification.Includes.Aggregate(
            query,
            (current, include) => current.Include(include));

        specification.IncludeStrings.Aggregate(
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

        return query;
    }

}