using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Specifications.Lockers;

public sealed class GetLockerDetailsSpec : Specification<Locker>
{
    public GetLockerDetailsSpec(Guid lockerId)
    {
        IgnoreQueryFilters();
        AddCriteria(l => l.Id == lockerId);

        AddInclude(l => l.Address);
        AddInclude(l => l.PricingPolicies);
        AddInclude(l => l.Cells);
        AddInclude("Cells.Business");
        AddInclude("Cells.BusinessGame");
        AddInclude("Cells.BusinessGame.Game");
    }
}