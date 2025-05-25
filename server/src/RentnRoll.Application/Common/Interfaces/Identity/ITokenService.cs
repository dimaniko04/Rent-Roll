using System.Security.Claims;

using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface ITokenService
{
    void SetTokenCookie(string token, bool isExpired = false);
    (string, string) GenerateTokens(UserResponse user);
    Result<ClaimsPrincipal?> GetTokenPrincipal(string token);
}