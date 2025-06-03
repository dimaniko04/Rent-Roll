using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;
using RentnRoll.Domain.Entities.Categories;
using RentnRoll.Domain.Entities.Genres;
using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Domain.Entities.Games;

public class Game : Entity, IAuditable
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public DateTime PublishedAt { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int Age { get; set; }
    public int? AveragePlayTime { get; set; }
    public int? ComplexityScore { get; set; }
    public bool IsVerified { get; set; } = true;
    public string? CreatedByUserId { get; set; }
    public string? VerifiedByUserId { get; set; }

    public ICollection<Image> Images { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];

    public ICollection<Genre> Genres { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Mechanic> Mechanics { get; set; } = [];

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}