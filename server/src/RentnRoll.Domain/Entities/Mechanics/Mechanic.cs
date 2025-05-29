using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Domain.Entities.Mechanics;

public class Mechanic : Entity
{
    public string Name { get; set; } = null!;

    public ICollection<Game> Games { get; set; } = [];
}