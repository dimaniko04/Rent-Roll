using RentnRoll.Application.Contracts.Businesses.CreateBusiness;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Contracts.Businesses.UpdateBusiness;

public record UpdateBusinessRequest(
    string Name,
    string Description
) : CreateBusinessRequest(Name, Description)
{
    public Business UpdateBusiness(Business business)
    {
        business.Name = Name;
        business.Description = Description;
        business.UpdatedAt = DateTime.UtcNow;

        return business;
    }
}