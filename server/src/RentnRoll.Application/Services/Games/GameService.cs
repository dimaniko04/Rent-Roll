using Microsoft.AspNetCore.Http;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.UserContext;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Games.CreateGame;
using RentnRoll.Application.Contracts.Games.GetAllGames;
using RentnRoll.Application.Contracts.Games.ReplaceGameImages;
using RentnRoll.Application.Contracts.Games.Response;
using RentnRoll.Application.Contracts.Games.UpdateGame;
using RentnRoll.Application.Services.FileStorage;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.Categories;
using RentnRoll.Application.Specifications.Games;
using RentnRoll.Application.Specifications.Genres;
using RentnRoll.Application.Specifications.Mechanics;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;
using RentnRoll.Domain.Entities.Categories;
using RentnRoll.Domain.Entities.Games;
using RentnRoll.Domain.Entities.Genres;
using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Services.Games;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;
    private readonly IValidationService _validationService;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IFileStorageService _fileStorageService;

    public GameService(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        IFileStorageService fileStorageService,
        ICurrentUserContext currentUserContext)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _currentUserContext = currentUserContext;
        _fileStorageService = fileStorageService;
        _gameRepository = unitOfWork
            .GetRepository<IGameRepository>();
    }

    public async Task<PaginatedResponse<GameNameResponse>> GetAllGameNamesAsync(
        GetAllGameNamesRequest request)
    {
        var specification = new GameSearchSpec(request);

        var gameNames = await _gameRepository
            .GetGameNamesAsync(specification);

        return gameNames;
    }

    public async Task<PaginatedResponse<GameResponse>> GetAllGamesAsync(
        GetAllGamesRequest request)
    {
        var specification = new GameSearchSpec(request);

        var games = await _gameRepository
            .GetPaginatedAsync(specification);

        return games;
    }

    public async Task<Result<GameDetailsResponse>> GetGameDetailsAsync(
        Guid gameId)
    {
        var specification = new GameDetailsSpec(gameId);

        var game = await _gameRepository
            .GetGameDetailsAsync(specification);

        if (game == null)
            return Errors.Games.NotFound;

        return game;
    }

    public async Task<Result<string>> UpdateGameThumbnailAsync(
        Game game,
        IFormFile file)
    {
        var url = await _fileStorageService
            .UploadAsync(file, "games/thumbnails");
        if (url.IsError)
            return url.Errors;

        if (game.ThumbnailUrl != null)
        {
            _fileStorageService.Delete(game.ThumbnailUrl);
        }

        game.ThumbnailUrl = url.Value!;
        _gameRepository.Update(game);
        await _unitOfWork.SaveChangesAsync();

        return game.ThumbnailUrl;
    }

    public async Task<Result<GameResponse>> CreateGameAsync(
        CreateGameRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var roles = _currentUserContext.Roles;
        var userId = _currentUserContext.UserId;
        var isAdmin = roles
            .Contains(Roles.Admin);

        var game = request.ToGame(isAdmin, userId);
        await _gameRepository.CreateAsync(game);
        await _unitOfWork.SaveChangesAsync();

        var gameResponse = GameResponse
            .FromGame(game);

        return gameResponse;
    }

    public async Task<Result<GameDetailsResponse>> UpdateGameAsync(
        Game game,
        UpdateGameRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var genres = await GetGenresAsync(request.Genres);
        if (genres.IsError)
            return genres.Errors;

        var categories = await GetCategoriesAsync(request.Categories);
        if (categories.IsError)
            return categories.Errors;

        var mechanics = await GetMechanicsAsync(request.Mechanics);
        if (mechanics.IsError)
            return mechanics.Errors;

        request.UpdateGame(
            game,
            genres.Value!,
            categories.Value!,
            mechanics.Value!);
        await _unitOfWork.SaveChangesAsync();

        var gameResponse = GameDetailsResponse.FromGame(game);

        return gameResponse;
    }

    public async Task<Result> DeleteGameAsync(Guid gameId)
    {
        var game = await _gameRepository.GetByIdAsync(gameId);
        if (game == null)
            return Result.Failure([Errors.Games.NotFound]);

        if (game.ThumbnailUrl != null)
            _fileStorageService.Delete(game.ThumbnailUrl);

        if (game.Images != null)
        {
            foreach (var image in game.Images)
            {
                _fileStorageService.Delete(image.Url);
            }
        }

        _gameRepository.Delete(game);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<List<string>>> AddGameImagesAsync(
        Game game, ICollection<IFormFile> files)
    {
        var images = new List<Image>();

        foreach (var file in files)
        {
            var uploadResult = await _fileStorageService
                .UploadAsync(file, "games/images");

            if (uploadResult.IsError)
            {
                foreach (var image in images)
                {
                    _fileStorageService.Delete(image.Url);
                }

                return uploadResult.Errors;
            }

            images.Add(new Image
            {
                Url = uploadResult.Value!,
                GameId = game.Id,
            });
        }
        foreach (var image in images)
        {
            game.Images.Add(image);
        }

        await _unitOfWork.SaveChangesAsync();

        return images
            .Select(i => i.Url)
            .ToList();
    }

    public async Task<Result<List<string>>> ReplaceGameImagesAsync(
        Game game, ReplaceGameImagesRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var notFound = request.UnmodifiedImagePaths
            .Except(game.Images.Select(i => i.Url))
            .ToList();
        if (notFound.Count != 0)
            return Errors.Games.ImagesNotFound(notFound);

        var toDelete = game.Images
                .Where(i => !request.UnmodifiedImagePaths.Contains(i.Url))
                .ToList();
        foreach (var image in toDelete)
        {
            var res = _fileStorageService.Delete(image.Url);
            if (res.IsError)
                return res.Errors;
            game.Images.Remove(image);
        }

        var urls = await AddGameImagesAsync(game, request.Files);
        if (urls.IsError)
            return urls.Errors;

        return request.UnmodifiedImagePaths
            .Concat(urls.Value!)
            .ToList();
    }

    public async Task<Result> DeleteGameImagesAsync(
        Game game,
        ICollection<string> imagePaths)
    {
        var notFound = imagePaths
            .Except(game.Images.Select(i => i.Url))
            .ToList();
        if (notFound.Count != 0)
            return Result.Failure([Errors.Games.ImagesNotFound(notFound)]);

        foreach (var path in imagePaths)
        {
            var res = _fileStorageService.Delete(path);
            if (res.IsError)
                return Result.Failure(res.Errors);
        }
        game.Images = game.Images
            .Where(i => !imagePaths.Contains(i.Url))
            .ToList();

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }


    private async Task<Result<List<Category>>> GetCategoriesAsync(
        ICollection<string>? categories)
    {
        if (categories == null || categories.Count == 0)
            return new List<Category>();

        var specification = new CategoryNamesSpec(categories);
        var categoryList = await _unitOfWork
            .GetRepository<ICategoryRepository>()
            .GetAllAsync(specification, trackChanges: true);

        if (categories.Count != categoryList.Count())
        {
            var missingCategories = categories
                .Except(categoryList.Select(c => c.Name))
                .ToList();
            return Errors.Games.CategoriesNotFound(missingCategories);
        }

        return categoryList.ToList();
    }

    private async Task<Result<List<Genre>>> GetGenresAsync(
        ICollection<string>? genres)
    {
        if (genres == null || genres.Count == 0)
            return new List<Genre>();

        var specification = new GenreNamesSpec(genres);
        var genreList = await _unitOfWork
            .GetRepository<IGenreRepository>()
            .GetAllAsync(specification, trackChanges: true);

        if (genres.Count != genreList.Count())
        {
            var missingGenres = genres
                .Except(genreList.Select(g => g.Name))
                .ToList();
            return Errors.Games.GenresNotFound(missingGenres);
        }

        return genreList.ToList();
    }

    private async Task<Result<List<Mechanic>>> GetMechanicsAsync(
        ICollection<string>? mechanics)
    {
        if (mechanics == null || mechanics.Count == 0)
            return new List<Mechanic>();

        var specification = new MechanicNamesSpec(mechanics);
        var mechanicList = await _unitOfWork
            .GetRepository<IMechanicRepository>()
            .GetAllAsync(specification, trackChanges: true);

        if (mechanics.Count != mechanicList.Count())
        {
            var missingMechanics = mechanics
                .Except(mechanicList.Select(m => m.Name))
                .ToList();
            return Errors.Games.MechanicsNotFound(missingMechanics);
        }

        return mechanicList.ToList();
    }
}