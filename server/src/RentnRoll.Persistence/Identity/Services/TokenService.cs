using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Identity.Services;

public class TokenService : ITokenService
{
    private readonly RentnRollDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public TokenService(
        RentnRollDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> AddUserRefreshTokenAsync(string userId)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Token = _jwtTokenGenerator.GenerateRefreshToken(),
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return refreshToken.Token;
    }
}