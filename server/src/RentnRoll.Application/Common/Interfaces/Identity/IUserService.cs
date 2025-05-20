using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface IUserService
{
    Task<Result<UserResponse>> GetCurrentUserAsync(string userId);
    Task<Result<UserResponse>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserRequest request);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<Result<UserResponse>> GetUserById(string userId);
    Task<Result<UserResponse>> CreateAdminUserAsync(
        UserRegisterRequest request);
    Task<Result<UserResponse>> PromoteUserToAdminAsync(
        string userId);
    Task<Result> BlockUserAsync(string userId);
    Task<Result> RestoreUserAsync(string userId);
}