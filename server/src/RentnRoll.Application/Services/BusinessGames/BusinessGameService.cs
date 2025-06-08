using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.UserContext;
using RentnRoll.Application.Contracts.BusinessGames.AddBusinessGame;
using RentnRoll.Application.Contracts.BusinessGames.DeleteBusinessGames;
using RentnRoll.Application.Contracts.BusinessGames.GetAllBusinessGames;
using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Application.Contracts.BusinessGames.UpdateBusinessGame;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.BusinessGames;
using RentnRoll.Application.Specifications.Tags;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Services.BusinessGames;

public class BusinessGameService : IBusinessGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IBusinessGameRepository _businessGameRepository;

    public BusinessGameService(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        ICurrentUserContext currentUserContext)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _currentUserContext = currentUserContext;
        _businessGameRepository = _unitOfWork
            .GetRepository<IBusinessGameRepository>();
    }

    public async Task<PaginatedResponse<BusinessGameResponse>>
        GetAllBusinessGamesAsync(
            Guid businessId,
            GetAllBusinessGamesRequest request)
    {
        var specification = new GetAllBusinessGamesRequestSpec(
            businessId, request);

        var businessGames = await _businessGameRepository
            .GetPaginatedAsync(specification);

        return businessGames;
    }

    public async Task<Result<BusinessGameResponse>>
        GetBusinessGameAsync(
            Guid businessId,
            Guid businessGameId)
    {
        var specification = new GetBusinessGameByIdSpec(
            businessId, businessGameId);
        var businessGame = await _businessGameRepository
            .GetSingleAsync(specification);

        if (businessGame is null)
            return Errors.BusinessGames.NotFound;

        var businessGameResponse = BusinessGameResponse
            .FromBusinessGame(businessGame);

        return businessGameResponse;
    }

    public async Task<Result<Guid>> AddBusinessGameAsync(
        Guid businessId,
        AddBusinessGameRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var tagsResult = await GetTagsAsync(businessId, request.Tags);
        if (tagsResult.IsError)
            return tagsResult.Errors;

        var businessGame = request.ToBusinessGame(
            businessId,
            tagsResult.Value!);
        await _businessGameRepository
            .CreateAsync(businessGame);
        await _unitOfWork.SaveChangesAsync();

        return businessGame.Id;
    }

    public async Task<Result<BusinessGameResponse>>
        UpdateBusinessGameAsync(
            Guid businessId,
            Guid businessGameId,
            UpdateBusinessGameRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var tagsResult = await GetTagsAsync(businessId, request.Tags);
        if (tagsResult.IsError)
            return tagsResult.Errors;

        var specification = new GetBusinessGameByIdSpec(
            businessId, businessGameId);
        var businessGame = await _businessGameRepository
            .GetSingleAsync(specification, trackChanges: true);

        if (businessGame is null)
            return Errors.BusinessGames.NotFound;

        request.UpdateBusinessGame(
            businessGame,
            tagsResult.Value!);
        await _unitOfWork.SaveChangesAsync();

        var businessGameResponse = BusinessGameResponse
            .FromBusinessGame(businessGame);

        return businessGameResponse;
    }

    public async Task<Result> DeleteBusinessGameAsync(
        Guid businessId,
        DeleteBusinessGamesRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return Result.Failure(validationResult.Errors);

        var gameIds = request.Ids;
        var specification = new GetBusinessGamesByIdsSpec(
            businessId,
            gameIds);
        var gamesList = await _businessGameRepository
            .GetAllAsync(specification);

        if (gameIds.Count != gamesList.Count())
        {
            var missingIds = gameIds
                .Except(gamesList.Select(bg => bg.Id))
                .ToList();
            return Result.Failure(
                [Errors.BusinessGames.IdsNotFound(missingIds)]);
        }

        await _businessGameRepository
            .DeleteRangeAsync(gamesList);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<Result<List<Tag>>> GetTagsAsync(
        Guid businessId,
        ICollection<string>? tags)
    {
        if (tags == null || tags.Count == 0)
            return new List<Tag>();

        var specification = new TagNamesSpec(businessId, tags);
        var tagsList = await _unitOfWork
            .GetRepository<ITagRepository>()
            .GetAllAsync(specification, trackChanges: true);

        if (tags.Count != tagsList.Count())
        {
            var missingTags = tags
                .Except(tagsList.Select(g => g.Name))
                .ToList();
            return Errors.BusinessGames.TagsNotFound(missingTags);
        }

        return tagsList.ToList();
    }
}