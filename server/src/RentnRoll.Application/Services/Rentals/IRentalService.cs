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
    Task<Result> CreateRentalAsync(
        CreateRentalRequest request);
    Task<Result> CancelRentalAsync();
    Task<Result> OpenCellAsync(string openReason);
    Task<Result> ConfirmStorePickUpAsync();
    Task<Result> SolveMaintenance(string solution);
}