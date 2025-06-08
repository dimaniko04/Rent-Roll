using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Domain.Entities.Lockers;

public class Locker : Entity
{
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public bool isActive { get; set; } = true;

    public ICollection<Cell> Cells { get; set; } = [];
    public ICollection<PricingPolicy> PricingPolicies { get; set; } = [];
}