using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.CreateRental;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;

namespace RentnRoll.Application.Services.Rentals;

public interface IRentalService
{
    Task<PaginatedResponse<RentalResponse>> GetAllRentalsAsync(
        GetAllRentalsRequest request);
    Task<RentalResponse> CreateRentalAsync(
        CreateRentalRequest request);
    Task<RentalResponse> CancelRentalAsync();
    Task<RentalResponse> OpenCellAsync(string openReason);
    Task<RentalResponse> ConfirmStorePickUpAsync();
    Task<RentalResponse> SolveMaintenance(string solution);
}