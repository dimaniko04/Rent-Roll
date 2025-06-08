using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Domain.Entities.Tags;

public class Tag : Entity, IAuditable
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public ICollection<BusinessGame> BusinessGames { get; set; } = [];
}