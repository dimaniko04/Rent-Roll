using RentnRoll.Domain.Common.Interfaces;

namespace RentnRoll.Domain.Entities.Games;

public class Review : IAuditable
{
    public int Rating { get; set; }
    public string Content { get; set; } = null!;

    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}