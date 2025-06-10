using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Stores.CreateStore;
using RentnRoll.Application.Contracts.Stores.GetAllStores;
using RentnRoll.Application.Contracts.Stores.Response;
using RentnRoll.Application.Contracts.Stores.UpdateStore;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.BusinessGames;
using RentnRoll.Application.Specifications.PricingPolicies;
using RentnRoll.Application.Specifications.Stores;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.PricingPolicies;

namespace RentnRoll.Application.Services.Stores;

public class StoreService : IStoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStoreRepository _storeRepository;
    private readonly IValidationService _validationService;
    private readonly ILogger<StoreService> _logger;

    public StoreService(
        IUnitOfWork unitOfWork,
        ILogger<StoreService> logger,
        IValidationService validationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _storeRepository = unitOfWork
            .GetRepository<IStoreRepository>();
    }

    public async Task<ICollection<StoreResponse>> GetAllStoresAsync(
        Guid businessId,
        GetAllStoresRequest request)
    {
        var specification = new GetAllStoresRequestSpec(
            businessId, request);
        var stores = await _storeRepository
            .GetAllAsync(specification, false);

        var storeResponses = stores
            .Select(StoreResponse.FromStore)
            .ToList();

        return storeResponses;
    }

    public async Task<Result<StoreDetailsResponse>> GetStoreByIdAsync(
        Guid businessId,
        Guid storeId)
    {
        var specification = new GetStoreDetailsSpec(
            businessId, storeId);
        var store = await _storeRepository
            .GetSingleAsync(specification, false);

        if (store == null)
            return Errors.Stores.NotFound;

        var storeResponse = StoreDetailsResponse
            .FromStore(store);

        return storeResponse;
    }

    public async Task<Result<StoreResponse>> CreateStoreAsync(
        Guid businessId,
        CreateStoreRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new StoreNameSpec(
            businessId, request.Name);
        var existingStore = await _storeRepository
            .GetSingleAsync(specification, false);

        if (existingStore != null)
            return Errors.Stores.AlreadyExists;

        var store = request.ToStore(businessId);

        await _storeRepository.CreateAsync(store);
        await _unitOfWork.SaveChangesAsync();

        var storeResponse = StoreResponse.FromStore(store);

        return storeResponse;
    }

    public async Task<Result<StoreDetailsResponse>> UpdateStoreAsync(
        Guid businessId,
        Guid storeId,
        UpdateStoreRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new GetStoreDetailsSpec(
            businessId, storeId);
        var store = await _storeRepository
            .GetSingleAsync(specification, trackChanges: true);
        if (store == null)
            return Errors.Stores.NotFound;

        var policyResult = await GetPolicyAsync(
            businessId, request.PolicyId);
        if (policyResult.IsError)
            return policyResult.Errors;

        var assetsValidationResult = await ValidateStoreAssetsAsync(
            businessId, request.Assets);
        if (assetsValidationResult.IsError)
            return assetsValidationResult.Errors;

        request.UpdateStore(store, policyResult.Value);
        await _unitOfWork.SaveChangesAsync();

        var response = StoreDetailsResponse
            .FromStore(store);

        return response;
    }

    public async Task<Result> DeleteStoreAsync(
        Guid businessId,
        Guid storeId)
    {
        var specification = new GetStoreDetailsSpec(
            businessId, storeId);
        var store = await _storeRepository
            .GetSingleAsync(specification);

        if (store == null)
            return Result.Failure([Errors.Stores.NotFound]);

        _storeRepository.Delete(store);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result<PricingPolicy?>> GetPolicyAsync(
        Guid businessId, Guid? policyId)
    {
        if (policyId is null)
            return (PricingPolicy?)null;

        var policySpec = new GetPricingPolicyDetailsSpec(
            businessId, policyId.Value);
        var policy = await _unitOfWork
            .GetRepository<IPricingPolicyRepository>()
            .GetSingleAsync(policySpec);

        if (policy == null)
            return Errors.Stores.PolicyNotFound(policyId.Value);

        return policy;
    }

    private async Task<Result> ValidateStoreAssetsAsync(
        Guid businessId,
        ICollection<UpdateStoreAssetRequest>? assets)
    {
        if (assets == null || !assets.Any())
            return Result.Success();

        var businessGameIds = assets
            .Select(i => i.BusinessGameId)
            .ToList();
        var spec = new GetBusinessGamesByIdsSpec(
            businessId, businessGameIds);
        var games = await _unitOfWork
            .GetRepository<IBusinessGameRepository>()
            .GetAllAsync(spec);

        if (businessGameIds.Count != games.Count())
        {
            var missingGames = businessGameIds
                .Except(games.Select(g => g.Id))
                .ToList();
            return Result.Failure(
                [Errors.Stores.GamesNotFound(missingGames)]);
        }

        return Result.Success();
    }
}