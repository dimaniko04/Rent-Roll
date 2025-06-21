using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.CreateRental;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Rentals;

public interface IRentalService
{
    Task<PaginatedResponse<RentalResponse>> GetAllRentalsAsync(
        GetAllRentalsRequest request);
    Task<ICollection<UserRentalResponse>> GetAllUserRentalsAsync(
        string userId);
    Task<Result> CreateRentalAsync(
        CreateRentalRequest request);
    Task<Result> CancelRentalAsync(Guid rentalId);
    Task<Result> OpenCellAsync(
        Guid rentalId,
        string openReason);
    Task<Result> ConfirmStorePickUpAsync(
        Guid rentalId);
    Task<Result> ConfirmStoreReturnAsync(
        Guid rentalId);
    Task<Result> SolveMaintenance(
        Guid rentalId,
        string solution);
}