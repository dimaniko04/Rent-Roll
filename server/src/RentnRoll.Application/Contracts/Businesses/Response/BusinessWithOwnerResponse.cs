using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Contracts.Businesses.Response;

public record BusinessWithOwnerResponse(
    Guid Id,
    string Name,
    string Description,
    string OwnerId,
    string OwnerName,
    string OwnerEmail,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsDeleted,
    DateTime? DeletedAt
) : BusinessResponse(
    Id,
    Name,
    Description)
{
    public static BusinessWithOwnerResponse FromBusiness(
        Business business,
        UserResponse owner)
        => new(
            business.Id,
            business.Name,
            business.Description,
            owner.Id,
            owner.LastName + " " + owner.FirstName,
            owner.Email,
            business.CreatedAt,
            business.UpdatedAt,
            business.IsDeleted,
            business.DeletedAt);
};
