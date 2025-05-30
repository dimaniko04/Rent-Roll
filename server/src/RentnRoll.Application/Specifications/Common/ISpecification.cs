using System.Linq.Expressions;

namespace RentnRoll.Application.Specifications.Common;

public interface ISpecification<T>
{
    List<Expression<Func<T, bool>>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    int PageNumber { get; }
    int PageSize { get; }
    bool IsPagingEnabled { get; }
}