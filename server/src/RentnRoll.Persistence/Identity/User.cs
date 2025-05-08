using Microsoft.AspNetCore.Identity;

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
}