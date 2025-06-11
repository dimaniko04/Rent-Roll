using RentnRoll.Application.Contracts.PricingPolicies.Response;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Contracts.Lockers.Response;

public record LockerDetailsResponse(
    Guid Id,
    string Name,
    Address Address,
    ICollection<CellResponse> Cells,
    ICollection<PricingPolicyResponse> PricingPolicies,
    bool? IsDeleted = null,
    DateTime? DeletedAt = null
)
{
    public static LockerDetailsResponse FromLocker(Locker locker)
    {
        return new LockerDetailsResponse(
            locker.Id,
            locker.Name,
            locker.Address,
            [.. locker
                .Cells
                .Select(CellResponse.FromCell)],
            [.. locker
                .PricingPolicies
                .Select(PricingPolicyResponse.FromPricingPolicy)],
            locker.IsDeleted,
            locker.DeletedAt
        );
    }
}