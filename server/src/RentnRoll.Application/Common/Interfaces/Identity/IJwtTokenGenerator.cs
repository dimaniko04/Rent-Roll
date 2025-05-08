using RentnRoll.Application.Contracts.Users;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserResponse user);
}