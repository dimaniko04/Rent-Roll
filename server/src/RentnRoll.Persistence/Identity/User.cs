using Microsoft.AspNetCore.Identity;

using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common.Interfaces;

namespace RentnRoll.Persistence.Identity;

public class User : IdentityUser, ISoftDeletable, IAuditable
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Country { get; set; }
    public DateTime? BirthDate { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    internal UserResponse ToUserResponse(IEnumerable<string> roles)
    {
        return new UserResponse(
            Id,
            Email!,
            FirstName,
            LastName,
            PhoneNumber,
            Country,
            BirthDate,
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
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            Country = request.Country,
            BirthDate = request.BirthDate,
            CreatedAt = DateTime.UtcNow
        };
    }
}