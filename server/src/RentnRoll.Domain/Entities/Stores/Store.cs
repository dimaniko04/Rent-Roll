using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Domain.Entities.Stores;

public class Store : Entity, IAuditable
{
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;

    public Guid? PolicyId { get; set; }
    public PricingPolicy? Policy { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<StoreAsset> Assets { get; set; } = [];
}