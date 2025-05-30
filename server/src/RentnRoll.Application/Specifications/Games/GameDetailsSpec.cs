using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Specifications.Games;

public sealed class GameDetailsSpec : Specification<Game>
{
    public GameDetailsSpec(Guid gameId)
    {
        AddCriteria(c => c.Id == gameId);

        AddInclude(c => c.Genres);
        AddInclude(c => c.Categories);
        AddInclude(c => c.Mechanics);
        AddInclude(c => c.Images);
    }
}