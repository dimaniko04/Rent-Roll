using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Specifications.Games;

public sealed class GameSearchSpec : Specification<Game>
{
    public GameSearchSpec(QueryParams request)
    {
        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(g => g.Name.Contains(request.Search));
        }

        ApplyOrderBy(g => g.Name);
        ApplyPaging(request.PageNumber, request.PageSize);
    }
}