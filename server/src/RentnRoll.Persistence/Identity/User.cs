using Microsoft.AspNetCore.Identity;

using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common.Interfaces;

namespace RentnRoll.Persistence.Identity;

public class User : IdentityUser, ISoftDeletable, IAuditable
{
    public string FullName { get; set; } = null!;
    public string? Country { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }


    internal UserResponse ToUserResponse()
    {
        return new UserResponse(
            Id,
            Email!,
            FullName!,
            Country
        );
    }

    internal DetailedUserResponse ToDetailedUserResponse(
        IEnumerable<string> roles)
    {
        return new DetailedUserResponse(
            Id,
            Email!,
            FullName!,
            Country,
            CreatedAt,
            UpdatedAt,
            DeletedAt,
            roles
        );
    }

    internal static User FromUserRegisterRequest(UserRegisterRequest request)
    {
        return new User
        {
            Email = request.Email,
            UserName = request.Email,
            Country = request.Country,
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow
        };
    }
}