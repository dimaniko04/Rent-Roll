using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Persistence.Settings;

namespace RentnRoll.Persistence.Identity.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(UserResponse user)
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
}