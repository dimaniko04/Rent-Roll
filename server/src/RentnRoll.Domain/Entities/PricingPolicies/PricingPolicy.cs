using RentnRoll.Domain.Common;
using RentnRoll.Domain.Common.Interfaces;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Domain.Entities.PricingPolicies;

public class PricingPolicy : Entity, IAuditable
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public TimeUnit TimeUnit { get; set; }
    public int UnitCount { get; set; } = 1;
    public int PricePercent { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;

    public ICollection<PricingPolicyItem> Items { get; set; } = [];
}