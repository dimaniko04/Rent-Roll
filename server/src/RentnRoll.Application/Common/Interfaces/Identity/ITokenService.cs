using RentnRoll.Application.Contracts.Users;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface ITokenService
{
    (string, string) GenerateTokens(UserResponse user);
    Task SaveRefreshTokenAsync(string userId, string refreshToken);
}