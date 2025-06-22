using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common;
using RentnRoll.Persistence.Settings;

namespace RentnRoll.Persistence.Identity.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(
        IOptions<JwtSettings> jwtOptions,
        IHttpContextAccessor httpContextAccessor)
    {
        _jwtSettings = jwtOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetTokenCookie(string token, bool isExpired = false)
    {
        var expires = isExpired
            ? DateTime.UtcNow.AddDays(-1)
            : DateTime.UtcNow.AddDays(
                _jwtSettings.RefreshTokenExpirationInDays);

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires,
        };

        _httpContextAccessor.HttpContext!
            .Response
            .Cookies
            .Append("refreshToken", token, options);
    }

    public (string, string) GenerateTokens(DetailedUserResponse user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        return (accessToken, refreshToken);
    }

    public Result<ClaimsPrincipal?> GetTokenPrincipal(string token)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = key,
        };

        try
        {
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch (Exception)
        {
            return Errors.Authentication.InvalidToken;
        }
    }

    private string GenerateAccessToken(DetailedUserResponse user)
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

        claims.AddRange(user.Roles
            .Select(role => new Claim(ClaimTypes.Role, role)));

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationInMinutes),
            signingCredentials: signingCredentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(securityToken);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

}