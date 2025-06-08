using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class PricingPolicyRepository
    : BaseRepository<PricingPolicy>, IPricingPolicyRepository
{
    public PricingPolicyRepository(RentnRollDbContext context)
        : base(context)
    {
    }
}