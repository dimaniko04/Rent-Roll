using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Domain.Entities.PricingPolicies;

public class PricingPolicyItem
{
    public Guid PolicyId { get; set; }
    public PricingPolicy Policy { get; set; } = null!;

    public Guid GameId { get; set; }
    public BusinessGame Game { get; set; } = null!;

    public int Price { get; set; }
}