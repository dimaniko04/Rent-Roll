using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(
        UserRegisterRequest request,
        string role = Roles.User);
    Task<Result<AuthResponse>> LoginAsync(
        UserLoginRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(
        string refreshToken);
    Task<Result> LogoutAsync(string userId);
}