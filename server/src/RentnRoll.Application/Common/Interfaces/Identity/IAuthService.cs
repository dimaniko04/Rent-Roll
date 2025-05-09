using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(
        UserRegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(
        UserLoginRequest request);
    Task<Result<AuthResponse>> RefreshTokenAsync(
        RefreshRequest request);
}