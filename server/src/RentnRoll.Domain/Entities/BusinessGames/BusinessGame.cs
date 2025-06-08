using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.Games;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Domain.Entities.BusinessGames;

public class BusinessGame : Entity
{
    public int Quantity { get; set; } = 1;
    public int BasePrice { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;

    public ICollection<Tag> Tags { get; set; } = [];
}