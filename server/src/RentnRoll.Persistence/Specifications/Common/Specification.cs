using System.Linq.Expressions;

namespace RentnRoll.Persistence.Specifications.Common;

public abstract class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void ApplyCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
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

    protected void ApplyPagination(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
        }

        PageNumber = pageNumber;
        PageSize = pageSize;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    const string DescendingSuffix = "desc";

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
        var normalizedPropertyName =
            propertyName.Substring(0, 1).ToUpper() +
            propertyName.Substring(1);

        var property = typeof(T).GetProperty(normalizedPropertyName) ??
            throw new InvalidOperationException(
                $"Property '{normalizedPropertyName}' does not exist on type '{typeof(T).Name}'.");
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
}
