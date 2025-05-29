using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Domain.Entities.Genres;

public class Genre : Entity
{
    public string Name { get; set; } = null!;

    public ICollection<Game> Games { get; set; } = [];
}