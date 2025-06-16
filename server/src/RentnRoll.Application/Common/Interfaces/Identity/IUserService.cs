using RentnRoll.Application.Contracts.Authentication;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.Interfaces.Identity;

public interface IUserService
{
    Task<Result<DetailedUserResponse>> GetCurrentUserAsync(
        string userId);
    Task<Result<ICollection<UserRentalResponse>>>
        GetCurrentUserRentalsAsync(string userId);
    Task<Result<DetailedUserResponse>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserRequest request);
    Task<PaginatedResponse<UserResponse>> GetAllUsersAsync(
        GetAllUsersRequest request);
    Task<Result<DetailedUserResponse>> GetUserById(
        string userId);
    Task<Result<DetailedUserResponse>> CreateAdminUserAsync(
        UserRegisterRequest request);
    Task<Result<DetailedUserResponse>> PromoteUserToAdminAsync(
        string userId);
    Task<Result> BlockUserAsync(string userId);
    Task<Result> RestoreUserAsync(string userId);
}