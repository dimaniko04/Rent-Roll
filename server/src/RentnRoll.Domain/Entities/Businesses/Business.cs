
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;

namespace RentnRoll.Domain.Entities.Businesses;

public class Business : Entity, ISoftDeletable, IAuditable
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public string OwnerId { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}