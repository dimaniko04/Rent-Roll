using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Specifications.Games;

public sealed class GameImageSpec : Specification<Game>
{
    public GameImageSpec(Guid gameId)
    {
        AddCriteria(c => c.Id == gameId);

        AddInclude(c => c.Images);
    }
}