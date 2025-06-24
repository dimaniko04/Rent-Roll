using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;
using RentnRoll.Application.Contracts.PricingPolicies.GetAllPricingPolicies;
using RentnRoll.Application.Contracts.PricingPolicies.Response;
using RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.BusinessGames;
using RentnRoll.Application.Specifications.PricingPolicies;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.PricingPolicies;

public class PricingPolicyService : IPricingPolicyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly IPricingPolicyRepository _pricingPolicyRepository;

    public PricingPolicyService(
        IUnitOfWork unitOfWork,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _pricingPolicyRepository = _unitOfWork
            .GetRepository<IPricingPolicyRepository>();
    }

    public async Task<List<PricingPolicyResponse>>
        GetAllPricingPoliciesAsync(
            Guid businessId,
            GetAllPricingPoliciesRequest request)
    {
        var specification = new GetAllPricingPoliciesRequestSpec(
            businessId, request);
        var policies = await _pricingPolicyRepository
            .GetAllAsync(specification);

        var policyResponses = policies
            .Select(PricingPolicyResponse.FromPricingPolicy)
            .ToList();

        return policyResponses;
    }

    public async Task<Result<PricingPolicyDetailsResponse>>
        GetPricingPolicyAsync(Guid businessId, Guid pricingPolicyId)
    {
        var specification = new GetPricingPolicyDetailsSpec(
            businessId, pricingPolicyId);
        var policy = await _pricingPolicyRepository
            .GetSingleAsync(specification);

        if (policy == null)
            return Errors.PricingPolicies.NotFound;

        var response = PricingPolicyDetailsResponse
            .FromPricingPolicy(policy);

        return response;
    }

    public async Task<Result<PricingPolicyDetailsResponse>>
        CreatePricingPolicyAsync(
            Guid businessId,
            CreatePricingPolicyRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var existingPoliciesSpec = new PricingPolicyNameSpec(
            businessId, request.Name);
        var existingPolicies = await _pricingPolicyRepository
            .GetSingleAsync(existingPoliciesSpec);

        if (existingPolicies is not null)
            return Errors.PricingPolicies.AlreadyExists(request.Name);

        var policy = request.ToPricingPolicy(businessId);
        await _pricingPolicyRepository.CreateAsync(policy);
        await _unitOfWork.SaveChangesAsync();

        var response = PricingPolicyDetailsResponse
            .FromPricingPolicy(policy);

        return response;
    }

    public async Task<Result<PricingPolicyResponse>>
        UpdatePricingPolicyAsync(
            Guid businessId,
            Guid pricingPolicyId,
            UpdatePricingPolicyRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new GetPricingPolicyDetailsSpec(
            businessId,
            pricingPolicyId);
        var policy = await _pricingPolicyRepository
            .GetSingleAsync(specification, trackChanges: true);

        if (policy == null)
            return Errors.PricingPolicies.NotFound;

        var itemsValidationResult = await ValidatePolicyItemsAsync(
            businessId, request.Items);
        if (itemsValidationResult.IsError)
            return itemsValidationResult.Errors;

        request.UpdatePricingPolicy(policy);
        await _unitOfWork.SaveChangesAsync();

        var response = PricingPolicyResponse
            .FromPricingPolicy(policy);

        return response;
    }

    public async Task<Result> DeletePricingPolicyAsync(
        Guid businessId, Guid pricingPolicyId)
    {
        var specification = new GetPricingPolicyDetailsSpec(
            businessId, pricingPolicyId);
        var policy = await _pricingPolicyRepository
            .GetSingleAsync(specification);

        if (policy == null)
            return Result.Failure([Errors.PricingPolicies.NotFound]);

        _pricingPolicyRepository.Delete(policy);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result> ValidatePolicyItemsAsync(
        Guid businessId,
        ICollection<UpdatePricingPolicyItemRequest>? items)
    {
        if (items == null || !items.Any())
            return Result.Success();

        var businessGameIds = items
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
                [Errors.PricingPolicies.GamesNotFound(missingGames)]);
        }

        return Result.Success();
    }
}