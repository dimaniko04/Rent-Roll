using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
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

    public LockerService(
        IUnitOfWork unitOfWork,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
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
}