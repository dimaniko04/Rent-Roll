
using System.Security.Cryptography.X509Certificates;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.UserContext;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.CreateRental;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;

namespace RentnRoll.Application.Services.Rentals;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;

    public RentalService(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public async Task<PaginatedResponse<RentalResponse>>
        GetAllRentalsAsync(GetAllRentalsRequest request)
    {
        var rentals = await _rentalRepository
            .GetAllRentalsAsync(request);

        return rentals;
    }

    public Task<RentalResponse> CreateRentalAsync(
        CreateRentalRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RentalResponse> CancelRentalAsync()
    {
        throw new NotImplementedException();
    }

    public Task<RentalResponse> ConfirmStorePickUpAsync()
    {
        throw new NotImplementedException();
    }

    public Task<RentalResponse> OpenCellAsync(string openReason)
    {
        throw new NotImplementedException();
    }

    public Task<RentalResponse> SolveMaintenance(string solution)
    {
        throw new NotImplementedException();
    }
}