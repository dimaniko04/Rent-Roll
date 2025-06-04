using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Contracts.Businesses.CreateBusiness;

public record CreateBusinessRequest(
    string Name,
    string Description)
{
    public Business ToBusiness(string ownerId) =>
        new()
        {
            Name = Name,
            Description = Description,
            OwnerId = ownerId,
        };
}