using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Specifications.Games;

public sealed class GameNameSpec : Specification<Game>
{
    public GameNameSpec(string name)
    {
        AddCriteria(g => g.Name == name);
    }
}