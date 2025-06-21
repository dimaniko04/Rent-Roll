
using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.UserContext;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.CreateRental;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Lockers.Enums;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;
using RentnRoll.Domain.Entities.Rentals;
using RentnRoll.Domain.Enums;

namespace RentnRoll.Application.Services.Rentals;

public class RentalService : IRentalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRentalRepository _rentalRepository;
    private readonly IValidationService _validationService;
    private readonly ICurrentUserContext _userContext;

    public RentalService(
        IUnitOfWork unitOfWork,
        ICurrentUserContext userContext,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _validationService = validationService;
        _rentalRepository = unitOfWork
            .GetRepository<IRentalRepository>();
    }

    public async Task<PaginatedResponse<RentalResponse>>
        GetAllRentalsAsync(GetAllRentalsRequest request)
    {
        var rentals = await _rentalRepository
            .GetAllRentalsAsync(request);

        return rentals;
    }

    public async Task<Result> CreateRentalAsync(
        CreateRentalRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);

        if (validationResult.IsError)
            return Result.Failure(
                validationResult.Errors);

        var userId = _userContext.UserId;
        var type = Enum.Parse<LocationType>(request.Type);
        var unit = Enum.Parse<TimeUnit>(request.Unit);

        var rental = new Rental
        {
            UserId = userId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow
                .AddHours(request.Term * (int)unit),
        };

        if (type == LocationType.Locker)
        {
            var cell = await _unitOfWork
                .GetRepository<ILockerRepository>()
                .GetCellByIdAsync(request.Id);

            if (cell == null)
            {
                return Result.Failure(
                    [Errors.Lockers.CellNotFound(request.Id)]);
            }

            if (cell.Status != CellStatus.Available)
            {
                return Result.Failure(
                    [Errors.Lockers.CellNotAvailable(request.Id)]);
            }

            var cellPolicy = await _unitOfWork
                .GetRepository<IPricingPolicyRepository>()
                .GetCellPolicyAsync(request.Id, unit);

            if (cellPolicy == null)
            {
                return Result.Failure(
                    [Errors.Rentals.CellPolicyNotFound(
                        request.Id,
                        request.Unit)]);
            }

            cell.Status = CellStatus.Reserved;
            rental.TotalPrice = cellPolicy.Price * request.Term;
            rental.LockerRental = new LockerRental
            {
                CellId = request.Id,
            };
        }
        else
        {
            var asset = await _unitOfWork
                .GetRepository<IStoreRepository>()
                .GetStoreAssetByIdAsync(request.Id);

            if (asset == null)
            {
                return Result.Failure(
                    [Errors.Stores.AssetNotFound(request.Id)]);
            }

            if (asset.Quantity <= 0)
            {
                return Result.Failure(
                    [Errors.Stores.OutOfStock(request.Id)]);
            }

            var assetPolicy = await _unitOfWork
                .GetRepository<IPricingPolicyRepository>()
                .GetAssetPolicyAsync(request.Id, unit);

            if (assetPolicy == null)
            {
                return Result.Failure(
                    [Errors.Rentals.AssetPolicyNotFound(
                        request.Id,
                        request.Unit)]);
            }

            asset.Quantity--;
            rental.TotalPrice = assetPolicy.Price * request.Term;
            rental.StoreRental = new StoreRental
            {
                StoreAssetId = request.Id,
            };
        }

        await _rentalRepository.CreateAsync(rental);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public Task<Result> CancelRentalAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> ConfirmStorePickUpAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> OpenCellAsync(string openReason)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SolveMaintenance(string solution)
    {
        throw new NotImplementedException();
    }
}