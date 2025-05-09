namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface ITokenService
{
    Task<string> AddUserRefreshTokenAsync(string userId);
}