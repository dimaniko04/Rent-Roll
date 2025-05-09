using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Settings;

namespace RentnRoll.Persistence.Identity.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RentnRollDbContext _dbContext;

    public TokenService(
        RentnRollDbContext dbContext,
        IOptions<JwtSettings> jwtOptions)
    {
        _dbContext = dbContext;
        _jwtSettings = jwtOptions.Value;
    }

    public (string, string) GenerateTokens(UserResponse user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        return (accessToken, refreshToken);
    }

    private string GenerateAccessToken(UserResponse user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            SecurityAlgorithms.HmacSha256
        );
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
        };
        if (user.Country != null)
        {
            claims.Add(new Claim(ClaimTypes.Country, user.Country));
        }
        if (user.BirthDate != null)
        {
            claims.Add(new Claim(
                ClaimTypes.DateOfBirth,
                user.BirthDate.ToString()!
            ));
        }

        claims.AddRange(user.Roles
            .Select(role => new Claim(ClaimTypes.Role, role)));

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpirationInMinutes),
            signingCredentials: signingCredentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(securityToken);
    }

    public async Task SaveRefreshTokenAsync(
        string userId,
        string tokenValue)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            Token = tokenValue,
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

}