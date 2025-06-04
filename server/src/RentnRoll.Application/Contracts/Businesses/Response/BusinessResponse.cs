using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Contracts.Businesses.Response;

public record BusinessResponse(
    Guid Id,
    string Name,
    string Description
)
{
    public static BusinessResponse FromBusiness(Business business) =>
        new(
            business.Id,
            business.Name,
            business.Description
        );
};