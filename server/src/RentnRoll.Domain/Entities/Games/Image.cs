using RentnRoll.Domain.Common.Interfaces;

namespace RentnRoll.Domain.Entities.Games;

public class Image : IAuditable
{
    public string Url { get; set; } = null!;

    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}