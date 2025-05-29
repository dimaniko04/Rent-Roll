using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Application.Specifications.Mechanics;
using RentnRoll.Domain.Common;
using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace RentnRoll.Application.Services.Mechanics;

public class MechanicService : IMechanicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMechanicRepository _MechanicRepository;
    private readonly IValidationService _validationService;

    public MechanicService(
        IUnitOfWork unitOfWork,
        ILogger<MechanicService> logger,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _MechanicRepository = unitOfWork
            .GetRepository<IMechanicRepository>();
    }

    public async Task<IEnumerable<MechanicResponse>> GetAllMechanicsAsync(
        GetAllMechanicsRequest request)
    {
        var specification = new GetAllMechanicsRequestSpec(request);

        var Mechanics = await _MechanicRepository
            .GetAllAsync(specification);
        var MechanicResponses = Mechanics
            .Select(MechanicResponse.FromMechanic);

        return MechanicResponses;
    }

    public async Task<Result<MechanicResponse>> GetMechanicByIdAsync(
        Guid MechanicId)
    {
        var Mechanic = await _MechanicRepository.GetByIdAsync(MechanicId);

        if (Mechanic == null)
            return Errors.Mechanics.NotFound;

        var mechanicResponse = MechanicResponse
            .FromMechanic(Mechanic);

        return mechanicResponse;
    }

    public async Task<Result<MechanicResponse>> CreateMechanicAsync(
        CreateMechanicRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var existingMechanic = await _MechanicRepository
            .GetByNameAsync(request.Name);

        if (existingMechanic != null)
            return Errors.Mechanics.AlreadyExists(request.Name);

        var Mechanic = request.ToMechanic();
        await _MechanicRepository.CreateAsync(Mechanic);
        await _unitOfWork.SaveChangesAsync();

        var mechanicResponse = MechanicResponse
            .FromMechanic(Mechanic);

        return mechanicResponse;
    }

    public async Task<Result<MechanicResponse>> UpdateMechanicAsync(
        Guid id,
        UpdateMechanicRequest request)
    {
        var validationResult = await _validationService
                    .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var Mechanic = await _MechanicRepository
            .GetByIdAsync(id);

        if (Mechanic == null)
            return Errors.Mechanics.NotFound;

        Mechanic.Name = request.Name;
        _MechanicRepository.Update(Mechanic);
        await _unitOfWork.SaveChangesAsync();

        var mechanicResponse = MechanicResponse
            .FromMechanic(Mechanic);

        return mechanicResponse;
    }

    public async Task<Result> DeleteMechanicAsync(Guid MechanicId)
    {
        var Mechanic = await _MechanicRepository.GetByIdAsync(MechanicId);
        if (Mechanic == null)
            return Result.Failure([Errors.Mechanics.NotFound]);

        _MechanicRepository.Delete(Mechanic);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}