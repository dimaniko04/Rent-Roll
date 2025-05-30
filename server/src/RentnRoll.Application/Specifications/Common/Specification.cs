using System.Linq.Expressions;
using System.Reflection;

namespace RentnRoll.Application.Specifications.Common;

public abstract class Specification<T> : ISpecification<T>
{
    public List<Expression<Func<T, bool>>> Criteria { get; private set; } = new();
    public List<Expression<Func<T, object>>> Includes { get; private set; } = new();
    public List<string> IncludeStrings { get; private set; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void ApplyCriteriaList(
        List<Expression<Func<T, bool>>> criteriaList)
    {
        foreach (var criteria in criteriaList)
        {
            AddCriteria(criteria);
        }
    }

    protected void AddCriteria(
        Expression<Func<T, bool>> criteria)
    {
        Criteria.Add(criteria);
    }

    protected void ApplyIncludeList(
        IEnumerable<Expression<Func<T, object>>> includes)
    {
        foreach (var include in includes)
        {
            AddInclude(include);
        }
    }

    protected void AddInclude(Expression<Func<T, object>> include)
    {
        Includes.Add(include);
    }

    protected void ApplyIncludeList(IEnumerable<string> includes)
    {
        foreach (var include in includes)
        {
            AddInclude(include);
        }
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    protected void ApplyPaging(int pageNumber, int pageSize)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        IsPagingEnabled = true;
    }

    const string DescendingSuffix = " desc";

    protected void ApplySorting(string sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return;
        }

        var isDescending = sort.EndsWith(
            DescendingSuffix,
            StringComparison.OrdinalIgnoreCase);

        var propertyName = sort.Split(' ')[0];
        var property = GetProperty(propertyName);

        if (property == null)
            return;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Convert(
            Expression.Property(parameter, property),
            typeof(object));

        var orderByExpression = Expression.Lambda<Func<T, object>>(
            propertyAccess,
            parameter);

        if (isDescending)
        {
            ApplyOrderByDescending(orderByExpression);
        }
        else
        {
            ApplyOrderBy(orderByExpression);
        }
    }

    private static PropertyInfo? GetProperty(string name)
    {
        var normalizedPropertyName =
            name.Substring(0, 1).ToUpper() +
            name.Substring(1);

        var property = typeof(T)
            .GetProperty(normalizedPropertyName);

        return property;
    }
}
