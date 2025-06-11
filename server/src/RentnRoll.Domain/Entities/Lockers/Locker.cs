
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Domain.Entities.Lockers;

public class Locker : Entity, IAuditable, ISoftDeletable
{
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;

    public ICollection<Cell> Cells { get; set; } = [];
    public ICollection<PricingPolicy> PricingPolicies { get; set; } = [];

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}