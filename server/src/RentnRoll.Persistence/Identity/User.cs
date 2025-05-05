using Microsoft.AspNetCore.Identity;

using RentnRoll.Domain.Common;

namespace RentnRoll.Persistence.Identity;

public class User : IdentityUser, ISoftDeletable
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}