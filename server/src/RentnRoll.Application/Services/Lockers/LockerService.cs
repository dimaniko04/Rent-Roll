using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Lockers.AssignBusiness;
using RentnRoll.Application.Contracts.Lockers.AssignGames;
using RentnRoll.Application.Contracts.Lockers.AssignPricingPolicy;
using RentnRoll.Application.Contracts.Lockers.ConfigureCells;
using RentnRoll.Application.Contracts.Lockers.CreateLocker;
using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Contracts.Lockers.Response;
using RentnRoll.Application.Contracts.Lockers.UpdateLocker;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.BusinessGames;
using RentnRoll.Application.Specifications.Lockers;
using RentnRoll.Application.Specifications.PricingPolicies;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.Entities.Lockers.Enums;

namespace RentnRoll.Application.Services.Lockers;

public class LockerService : ILockerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILockerRepository _lockerRepository;
    private readonly IValidationService _validationService;
    private readonly IMqttPublisher _mqttPublisher;

    public LockerService(
        IUnitOfWork unitOfWork,
        IMqttPublisher mqttPublisher,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mqttPublisher = mqttPublisher;
        _validationService = validationService;
        _lockerRepository = unitOfWork
            .GetRepository<ILockerRepository>();
    }

    public async Task<ICollection<LockerResponse>>
        GetAllLockersAsync(GetAllLockersRequest request)
    {
        var specification = new GetAllLockersRequestSpec(request);
        var lockers = await _lockerRepository
            .GetAllAsync(specification, false);

        var lockerResponses = lockers
            .Select(LockerResponse.FromLocker)
            .ToList();

        return lockerResponses;
    }

    public async Task<Result<LockerDetailsResponse>>
        GetLockerByIdAsync(Guid lockerId)
    {
        var specification = new GetLockerDetailsSpec(
            lockerId);
        var locker = await _lockerRepository
            .GetSingleAsync(specification, false);

        if (locker == null)
            return Errors.Lockers.NotFound;

        var lockerResponse = LockerDetailsResponse
            .FromLocker(locker);

        return lockerResponse;
    }

    public async Task<Result<LockerResponse>>
        CreateLockerAsync(CreateLockerRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new LockerNameSpec(request.Name);
        var existingLocker = await _lockerRepository
            .GetSingleAsync(specification, false);

        if (existingLocker != null)
            return Errors.Lockers.AlreadyExists(request.Name);

        var locker = request.ToLocker();

        await _lockerRepository.CreateAsync(locker);
        await _unitOfWork.SaveChangesAsync();

        var lockerResponse = LockerResponse.FromLocker(locker);

        return lockerResponse;
    }

    public async Task<Result<ICollection<CellResponse>>>
        ConfigureCellsAsync(
            Guid lockerId,
            ConfigureCellsRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var existingConfig = await _lockerRepository
            .GetCellsByIotDeviceIdAsync(request.DeviceId);

        if (existingConfig.Any())
            return Errors.Lockers.DeviceAlreadyConfigured(
                request.DeviceId);

        var locker = await _lockerRepository
            .GetByIdWithUnconfiguredCellsAsync(lockerId);

        if (locker == null)
            return Errors.Lockers.NotFound;

        var cells = locker.Cells.ToList();

        if (cells.Count < request.Pins.Count)
            return Errors.Lockers.NotEnoughCells(
                lockerId, cells.Count, request.Pins.Count);

        var configuration = request.Pins
            .Select((pin, index) => (cells[index].Id, pin))
            .ToList();

        await _mqttPublisher.PublishLockerConfigAsync(
            request.DeviceId, configuration);

        foreach (var cell in cells)
        {
            cell.IotDeviceId = request.DeviceId;
        }
        await _unitOfWork.SaveChangesAsync();

        var cellResponses = cells
            .Select(CellResponse.FromCell)
            .ToList();

        return cellResponses;
    }

    public async Task<Result> DeleteConfigurationAsync(
        string deviceId)
    {
        var locker = await _lockerRepository
            .GetByIotDeviceIdAsync(deviceId);

        if (locker == null)
            return Result.Failure(
                [Errors.Lockers.DeviceNotConfigured(deviceId)]);

        if (locker.Cells.Any(c => c.BusinessGameId != null))
            return Result.Failure(
                [Errors.Lockers.CellsNotEmpty(locker.Cells
                    .Where(c => c.BusinessGameId != null)
                    .Select(c => c.Id).ToList())]);

        await _mqttPublisher
            .PublishLockerConfigAsync(deviceId, []);

        foreach (var cell in locker.Cells)
        {
            cell.IotDeviceId = null;
        }
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<LockerDetailsResponse>>
        UpdateLockerAsync(Guid lockerId, UpdateLockerRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new GetLockerDetailsSpec(
            lockerId);
        var locker = await _lockerRepository
            .GetSingleAsync(specification, true);

        if (locker == null)
            return Errors.Lockers.NotFound;

        var cells = locker.Cells;
        var configuredCells = cells
            .Where(c => c.IotDeviceId != null ||
                        c.BusinessId != null)
            .ToList();

        if (configuredCells.Count > request.NumberOfCells)
            return Errors.Lockers.CannotDeleteActiveCells(
                configuredCells
                    .Select(c => c.Id)
                    .ToList());

        if (cells.Count > request.NumberOfCells)
        {
            cells = cells
                .OrderByDescending(c => c.IotDeviceId != null)
                .ThenByDescending(c => c.BusinessId != null)
                .Take(request.NumberOfCells)
                .ToList();
        }
        else
        {
            var quantityToAdd = request.NumberOfCells - cells.Count;
            var newCells = Enumerable
                .Range(1, quantityToAdd)
                .Select(i => new Cell())
                .ToList();
            cells = cells
                .Concat(newCells)
                .ToList();
        }

        locker.Name = request.Name;
        locker.Address = request.Address;
        locker.Cells = cells;

        await _unitOfWork.SaveChangesAsync();

        var lockerResponse = LockerDetailsResponse
            .FromLocker(locker);

        return lockerResponse;
    }

    public async Task<Result<LockerResponse>>
        DeactivateLockerAsync(Guid lockerId)
    {
        var locker = await _lockerRepository
            .GetByIdAsync(lockerId, true);

        if (locker == null)
            return Errors.Lockers.NotFound;

        locker.IsActive = false;
        await _unitOfWork.SaveChangesAsync();

        var lockerResponse = LockerResponse
            .FromLocker(locker);

        return lockerResponse;
    }

    public async Task<Result<LockerResponse>>
        ActivateLockerAsync(Guid lockerId)
    {
        var specification = new LockerIdSpec(lockerId);
        var locker = await _lockerRepository
            .GetSingleAsync(specification, true);

        if (locker == null)
            return Errors.Lockers.NotFound;

        locker.IsActive = true;
        await _unitOfWork.SaveChangesAsync();

        var lockerResponse = LockerResponse
            .FromLocker(locker);

        return lockerResponse;
    }

    public async Task<Result> DeleteLockerAsync(Guid lockerId)
    {
        var specification = new LockerIdSpec(lockerId);
        var locker = await _lockerRepository
            .GetSingleAsync(specification);

        if (locker == null)
            return Result.Failure([Errors.Lockers.NotFound]);

        var activeCells = locker.Cells
            .Where(c => c.Status == CellStatus.Reserved)
            .ToList();

        if (activeCells.Any())
            return Result.Failure(
                [Errors.Lockers.HasActiveRentals(lockerId)]);

        var nonEmptyCells = locker.Cells
            .Where(c => c.BusinessGameId != null)
            .ToList();

        if (nonEmptyCells.Any())
            return Result.Failure(
                [Errors.Lockers.CellsNotEmpty(nonEmptyCells
                    .Select(c => c.Id).ToList())]);

        foreach (var cell in locker.Cells)
        {
            if (cell.IotDeviceId != null)
            {
                await _mqttPublisher
                    .PublishLockerConfigAsync(
                        cell.IotDeviceId, []);
            }
        }

        _lockerRepository.Delete(locker);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<ICollection<CellResponse>>>
        AssignBusinessAsync(
            Guid lockerId,
            AssignBusinessRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var locker = await _lockerRepository
            .GetConfiguredUnassigned(lockerId);

        if (locker == null)
            return Errors.Lockers.NotFound;

        var unassigned = locker.Cells;

        if (unassigned.Count < request.CellCount)
            return Errors.Lockers.NotEnoughCells(
                lockerId, request.CellCount, unassigned.Count);

        unassigned = unassigned
            .OrderBy(c => c.IotDeviceId != null)
            .Take(request.CellCount)
            .ToList();

        foreach (var cell in unassigned)
        {
            cell.BusinessId = request.BusinessId;
        }

        await _unitOfWork.SaveChangesAsync();

        var cellResponses = unassigned
            .Select(CellResponse.FromCell)
            .ToList();

        return cellResponses;
    }

    public async Task<Result<ICollection<CellResponse>>>
        AssignGamesAsync(
            Guid lockerId,
            Guid businessId,
            AssignGamesRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var cellIds = request.GameAssignments
            .Select(ga => ga.CellId)
            .ToList();
        var gameIds = request.GameAssignments
            .Select(ga => ga.BusinessGameId)
            .ToList();

        var locker = await _lockerRepository
            .GetCellsByIdsAsync(lockerId, cellIds);

        if (locker == null)
            return Errors.Lockers.NotFound;

        var cells = locker.Cells;

        if (cells.Count != cellIds.Count)
            return Errors.Lockers.CellsNotFound(
                cellIds.Except(cells.Select(c => c.Id)).ToList());

        if (cells.Any(c => c.BusinessId != businessId))
            return Errors.Lockers.CellsNotBelongToBusiness(
                businessId,
                cells.Where(c => c.BusinessId != businessId)
                    .Select(c => c.Id).ToList());

        var specification = new GetBusinessGamesByIdsSpec(
            businessId, gameIds);
        var games = await _unitOfWork
            .GetRepository<IBusinessGameRepository>()
            .GetAllAsync(specification);

        if (games.Count() != gameIds.Count)
            return Errors.BusinessGames.IdsNotFound(
                [.. gameIds.Except(games.Select(g => g.Id))]);

        cells = cells.Join(
            request.GameAssignments,
            cell => cell.Id,
            assignment => assignment.CellId,
            (cell, assignment) =>
            {
                cell.BusinessGameId = assignment.BusinessGameId;
                cell.Status = CellStatus.Available;
                return cell;
            })
            .ToList();

        await _unitOfWork.SaveChangesAsync();

        var cellResponses = cells
            .Select(CellResponse.FromCell)
            .ToList();

        return cellResponses;
    }

    public async Task<Result<LockerDetailsResponse>> AssignPricingPolicyAsync(
        Guid lockerId,
        Guid businessId,
        AssignPricingPolicyRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new GetLockerDetailsSpec(lockerId);
        var locker = await _lockerRepository
            .GetSingleAsync(specification, true);

        if (locker == null)
            return Errors.Lockers.NotFound;

        var policySpecification = new GetPricingPolicyDetailsSpec(
            businessId,
            request.PricingPolicyId);
        var pricingPolicy = await _unitOfWork
            .GetRepository<IPricingPolicyRepository>()
            .GetSingleAsync(policySpecification, false);

        if (pricingPolicy == null)
            return Errors.PricingPolicies.NotFound;


        var existingPolicy = locker.PricingPolicies.FirstOrDefault(
            p => p.BusinessId == businessId);

        if (existingPolicy != null)
        {
            locker.PricingPolicies.Remove(existingPolicy);
        }

        locker.PricingPolicies.Add(pricingPolicy);

        var lockerResponse = LockerDetailsResponse
            .FromLocker(locker);

        return lockerResponse;
    }
}