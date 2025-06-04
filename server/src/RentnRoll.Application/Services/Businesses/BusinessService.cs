using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.UserContext;
using RentnRoll.Application.Contracts.Businesses.CreateBusiness;
using RentnRoll.Application.Contracts.Businesses.GetAllBusinesses;
using RentnRoll.Application.Contracts.Businesses.Response;
using RentnRoll.Application.Contracts.Businesses.UpdateBusiness;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.Businesses;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Services.Businesses;

public class BusinessService : IBusinessService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IBusinessRepository _businessRepository;

    public BusinessService(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        ICurrentUserContext currentUserContext)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _currentUserContext = currentUserContext;
        _businessRepository = _unitOfWork
            .GetRepository<IBusinessRepository>();
    }

    public async Task<PaginatedResponse<BusinessWithOwnerResponse>>
        GetPaginatedAsync(GetAllBusinessesRequest request)
    {
        var specification = new GetAllBusinessRequestSpec(request);
        var businesses = await _businessRepository
            .GetPaginatedWithOwnerAsync(specification);

        return businesses;
    }

    public async Task<Result<BusinessWithOwnerResponse>>
        GetByIdAsync(Guid id)
    {
        var specification = new GetBusinessByIdWithOwnerSpec(id);
        var businessWithOwner = await _businessRepository
            .GetSingleWithOwnerAsync(specification);

        if (businessWithOwner is null)
        {
            return Errors.Businesses.NotFound;
        }

        return businessWithOwner;
    }

    public async Task<Result> BlockAsync(Guid businessId)
    {
        var business = await _businessRepository
            .GetByIdAsync(businessId);

        if (business is null)
            return Result.Failure([Errors.Businesses.NotFound]);

        _businessRepository.Delete(business);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> RestoreAsync(Guid businessId)
    {
        var specification = new BusinessIdSpec(businessId);
        var business = await _businessRepository
            .GetSingleAsync(specification, trackChanges: true);

        if (business is null)
            return Result.Failure([Errors.Businesses.NotFound]);

        business.IsDeleted = false;
        business.DeletedAt = null;
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<BusinessResponse>> GetMyBusinessAsync()
    {
        var result = await GetOwnBusinessAsync();

        if (result.IsError)
            return result.Errors;

        var business = BusinessResponse
            .FromBusiness(result.Value!);

        return business;
    }


    public async Task<Result<BusinessResponse>> CreateAsync(
        CreateBusinessRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var result = await GetOwnBusinessAsync();
        if (result.IsSuccess)
            return Errors.Businesses.UserAlreadyHasBusiness;

        var userId = _currentUserContext.UserId;
        var business = request.ToBusiness(userId);
        await _businessRepository.CreateAsync(business);
        await _unitOfWork.SaveChangesAsync();

        var businessResponse = BusinessResponse
            .FromBusiness(business);

        return businessResponse;
    }

    public async Task<Result<BusinessResponse>> UpdateAsync(
        UpdateBusinessRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var result = await GetOwnBusinessAsync();
        if (result.IsError)
            return result.Errors;

        var business = request.UpdateBusiness(result.Value!);
        await _unitOfWork.SaveChangesAsync();

        var businessResponse = BusinessResponse
            .FromBusiness(business);

        return businessResponse;
    }

    private async Task<Result<Business>> GetOwnBusinessAsync()
    {
        var userId = _currentUserContext.UserId;
        var specification = new OwnerIdSpec(userId);
        var business = await _businessRepository
            .GetSingleAsync(specification, trackChanges: true);

        if (business is null)
            return Errors.Businesses.NotFound;

        if (business.IsDeleted)
            return Errors.Businesses.Blocked;

        return business;
    }
}