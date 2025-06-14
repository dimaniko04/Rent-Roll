using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Application.Contracts.Lockers.Response;

public record BusinessLockerResponse(
    Guid Id,
    string Name,
    string Address,
    bool IsActive,
    ICollection<CellResponse> Cells,
    string? PricingPolicyName
)
{
    public static BusinessLockerResponse FromLocker(Locker locker)
    {
        return new BusinessLockerResponse(
            locker.Id,
            locker.Name,
            locker.Address.ToString(),
            locker.IsActive,
            [.. locker
                .Cells
                .Select(CellResponse.FromCell)],
            locker
                .PricingPolicies
                .FirstOrDefault()?
                .Name
        );
    }
}

