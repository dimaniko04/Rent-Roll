using RentnRoll.Domain.Common;

namespace RentnRoll.Persistence.Identity;

public class RefreshToken : Entity
{
    public DateTime ExpiresAt { get; set; }
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;

    public User User { get; set; } = null!;
}