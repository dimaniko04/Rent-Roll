
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
using RentnRoll.Domain.Entities.Rentals.Enums;
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

    public async Task<ICollection<UserRentalResponse>>
        GetAllUserRentalsAsync(string userId)
    {
        var rentals = await _rentalRepository
            .GetAllUserRentalsAsync(userId);

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

    public async Task<Result> CancelRentalAsync(Guid id)
    {
        var userId = _userContext.UserId;
        var rental = await _rentalRepository
            .GetByIdAsync(id, trackChanges: true);

        if (rental == null || rental.UserId != userId)
        {
            return Result.Failure(
                [Errors.Rentals.RentalNotFound(id)]);
        }

        if (rental.Status != RentalStatus.Expectation)
        {
            return Result.Failure(
                [Errors.Rentals.RentalAlreadyActive(id)]);
        }

        if (rental.LockerRental != null)
        {
            var cell = await _unitOfWork
                .GetRepository<ILockerRepository>()
                .GetCellByIdAsync(rental.LockerRental.CellId);

            if (cell == null)
            {
                return Result.Failure(
                    [Errors.Lockers.CellNotFound(rental.LockerRental.CellId)]);
            }

            cell.Status = CellStatus.Available;
        }
        else
        {
            var asset = await _unitOfWork
                .GetRepository<IStoreRepository>()
                .GetStoreAssetByIdAsync(rental.StoreRental!.StoreAssetId);

            if (asset == null)
            {
                return Result.Failure(
                    [Errors.Stores.AssetNotFound(rental.StoreRental.StoreAssetId)]);
            }

            asset.Quantity++;
        }

        rental.Status = RentalStatus.Cancelled;
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public Task<Result> OpenCellAsync(
        Guid rentalId,
        string openReason)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ConfirmStorePickUpAsync(
        Guid rentalId)
    {
        var rental = await _rentalRepository
            .GetByIdAsync(rentalId, trackChanges: true);

        if (rental == null)
        {
            return Result.Failure(
                [Errors.Rentals.RentalNotFound(rentalId)]);
        }

        var isOwner = await IsStoreOwner(rental);
        if (isOwner.IsError)
        {
            return isOwner;
        }

        if (rental.Status != RentalStatus.Expectation)
        {
            return Result.Failure(
                [Errors.Rentals.RentalAlreadyActive(rentalId)]);
        }

        rental.Status = RentalStatus.Active;
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ConfirmStoreReturnAsync(
        Guid rentalId)
    {
        var rental = await _rentalRepository
            .GetByIdAsync(rentalId, trackChanges: true);

        if (rental == null)
        {
            return Result.Failure(
                [Errors.Rentals.RentalNotFound(rentalId)]);
        }

        var isOwner = await IsStoreOwner(rental);
        if (isOwner.IsError)
        {
            return isOwner;
        }

        if (rental.Status != RentalStatus.Active &&
            rental.Status != RentalStatus.Overdue)
        {
            return Result.Failure(
                [Errors.Rentals.RentalNotActive(rentalId)]);
        }

        var asset = await _unitOfWork
               .GetRepository<IStoreRepository>()
               .GetStoreAssetByIdAsync(rental.StoreRental!.StoreAssetId);

        if (asset == null)
        {
            return Result.Failure(
                [Errors.Stores.AssetNotFound(rental.StoreRental.StoreAssetId)]);
        }

        asset.Quantity++;
        rental.Status = RentalStatus.Returned;
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> SolveMaintenance(
        Guid rentalId, string solution)
    {
        var rental = await _rentalRepository
            .GetByIdAsync(rentalId, trackChanges: true);

        if (rental == null)
        {
            return Result.Failure(
                [Errors.Rentals.RentalNotFound(rentalId)]);
        }

        var isOwner = await IsCellOwner(rental);
        if (isOwner.IsError)
        {
            return isOwner;
        }

        var cell = rental.LockerRental?.Cell;

        if (cell?.Status != CellStatus.Maintenance)
        {
            return Result.Failure(
                [Errors.Rentals.CellNotInMaintenance(rentalId)]);
        }

        if (!Enum.TryParse<MaintenanceSolution>(solution, out var parsedSolution))
        {
            return Result.Failure(
                [Error.InvalidRequest(
                    "Rentals.InvalidMaintenanceSolution",
                    $"Invalid maintenance solution: {solution}. Expected \"GameAvailable\" or \"GameLost\" ")]);
        }

        if (parsedSolution == MaintenanceSolution.GameAvailable)
        {
            cell.Status = CellStatus.Available;
        }
        else if (parsedSolution == MaintenanceSolution.GameLost)
        {
            cell.Status = CellStatus.Empty;
            cell.BusinessGame = null;
        }

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result> IsStoreOwner(Rental rental)
    {
        if (rental.StoreRental?.StoreAsset?.StoreId == null)
        {
            return Result.Failure(
                [Errors.Rentals.NoRelatedStore(rental.Id)]);
        }

        var business = await _unitOfWork
            .GetRepository<IBusinessRepository>()
            .GetByOwnerIdAsync(_userContext.UserId);
        var store = await _unitOfWork
            .GetRepository<IStoreRepository>()
            .GetByIdAsync(rental.StoreRental.StoreAsset.StoreId);

        if (business == null ||
            store == null ||
            business.Id != store.BusinessId)
        {
            return Result.Failure(
                [Errors.Businesses.NotOwner]);
        }

        return Result.Success();
    }

    private async Task<Result> IsCellOwner(Rental rental)
    {
        if (rental.LockerRental?.Cell == null)
        {
            return Result.Failure(
                [Errors.Rentals.NoRelatedStore(rental.Id)]);
        }

        var cell = rental.LockerRental.Cell;
        var business = await _unitOfWork
            .GetRepository<IBusinessRepository>()
            .GetByOwnerIdAsync(_userContext.UserId);

        if (business == null ||
            business.Id != cell.BusinessId)
        {
            return Result.Failure(
                [Errors.Businesses.NotOwner]);
        }

        return Result.Success();
    }
}