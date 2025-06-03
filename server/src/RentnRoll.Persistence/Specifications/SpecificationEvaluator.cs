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

        if (specification.Criteria.Count != 0)
        {
            foreach (var criteria in specification.Criteria)
            {
                query = query.Where(criteria);
            }
        }

        if (specification.Includes.Count != 0)
        {
            foreach (var include in specification.Includes)
            {
                query = query.Include(include);
            }
        }

        if (specification.IncludeStrings.Count != 0)
        {
            foreach (var include in specification.IncludeStrings)
            {
                query = query.Include(include);
            }
        }

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