
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;
using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Domain.Entities.Businesses;

public class Business : Entity, ISoftDeletable, IAuditable
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public string OwnerId { get; set; } = null!;
    public ICollection<BusinessGame> Games { get; set; } = [];
    public ICollection<PricingPolicy> Policies { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
    public ICollection<Store> Stores { get; set; } = [];

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}