using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Lockers.ConfigureCells;
using RentnRoll.Application.Contracts.Lockers.CreateLocker;
using RentnRoll.Application.Contracts.Lockers.GetAllLockers;
using RentnRoll.Application.Contracts.Lockers.Response;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.Lockers;
using RentnRoll.Domain.Common;

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
}