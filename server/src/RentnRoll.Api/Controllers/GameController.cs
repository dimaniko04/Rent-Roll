using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.Games.CreateGame;
using RentnRoll.Application.Contracts.Games.GetAllGames;
using RentnRoll.Application.Contracts.Games.ReplaceGameImages;
using RentnRoll.Application.Contracts.Games.UpdateGame;
using RentnRoll.Application.Services.Games;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Application.Specifications.Games;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/games")]
[Authorize]
public class GameController : ApiController
{
    private readonly IGameService _gameService;
    private readonly IGameRepository _gameRepository;
    private readonly IAuthorizationService _authorizationService;

    public GameController(
        IGameService gameService,
        IGameRepository gameRepository,
        IAuthorizationService authorizationService)
    {
        _gameService = gameService;
        _gameRepository = gameRepository;
        _authorizationService = authorizationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGames(
        [FromQuery] GetAllGamesRequest request)
    {
        var result = await _gameService
            .GetAllGamesAsync(request);

        return Ok(result);
    }

    [HttpGet("{gameId:guid}")]
    public async Task<IActionResult> GetGameById(Guid gameId)
    {
        var result = await _gameService
            .GetGameDetailsAsync(gameId);

        return result.Match(Ok, Problem);
    }

    [HttpGet("names")]
    public async Task<IActionResult> GetAllGameNames(
        [FromQuery] GetAllGameNamesRequest request)
    {
        var result = await _gameService
            .GetAllGameNamesAsync(request);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Business}")]
    public async Task<IActionResult> CreateGame(
        CreateGameRequest request)
    {
        var result = await _gameService
            .CreateGameAsync(request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{gameId:guid}")]
    public async Task<IActionResult> UpdateGame(
        Guid gameId,
        UpdateGameRequest request)
    {
        var specification = new GameDetailsSpec(gameId);
        var authorizeResult = await AuthorizeForGameAsync(
            specification, trackChanges: true);
        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var game = authorizeResult.Value!;
        var result = await _gameService
            .UpdateGameAsync(game, request);
        return result.Match(Ok, Problem);
    }

    [HttpPost("{gameId:guid}/thumbnail")]
    public async Task<IActionResult> UploadGameThumbnail(
        Guid gameId,
        [FromForm] IFormFile thumbnail)
    {
        var authorizeResult = await AuthorizeForGameAsync(gameId);
        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var game = authorizeResult.Value!;
        var result = await _gameService
            .UpdateGameThumbnailAsync(game, thumbnail);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{gameId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGame(Guid gameId)
    {
        var result = await _gameService
            .DeleteGameAsync(gameId);
        return result.Match(Ok, Problem);
    }

    [HttpPost("{gameId:guid}/images")]
    public async Task<IActionResult> AddGameImages(
        Guid gameId,
        [FromForm] ICollection<IFormFile> files)
    {
        var specification = new GameImageSpec(gameId);
        var authorizeResult = await AuthorizeForGameAsync(
            specification, trackChanges: true);
        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var game = authorizeResult.Value!;
        var result = await _gameService
            .AddGameImagesAsync(game, files);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{gameId:guid}/images")]
    public async Task<IActionResult> ReplaceGameImages(
        Guid gameId,
        [FromForm] ReplaceGameImagesRequest request)
    {
        var specification = new GameImageSpec(gameId);
        var authorizeResult = await AuthorizeForGameAsync(
            specification, trackChanges: true);
        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var game = authorizeResult.Value!;
        var result = await _gameService
            .ReplaceGameImagesAsync(game, request);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{gameId:guid}/images")]
    public async Task<IActionResult> DeleteGameImages(
        Guid gameId,
        [FromBody] ICollection<string> imagePaths)
    {
        var specification = new GameImageSpec(gameId);
        var authorizeResult = await AuthorizeForGameAsync(
            specification, trackChanges: true);
        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var game = authorizeResult.Value!;
        var result = await _gameService
            .DeleteGameImagesAsync(game, imagePaths);
        return result.Match(Ok, Problem);
    }

    private Result<Game> AuthorizeForGame(Game? game)
    {
        if (game is null)
            return Errors.Games.NotFound;

        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, game, Policy.CreatorOrAdmin).Result;

        if (!authorizationResult.Succeeded)
            return Errors.User.Unauthorized;

        return game;
    }

    private async Task<Result<Game>> AuthorizeForGameAsync(Guid gameId)
    {
        var game = await _gameRepository.GetByIdAsync(gameId);
        return AuthorizeForGame(game);
    }

    private async Task<Result<Game>> AuthorizeForGameAsync(
        ISpecification<Game> specification,
        bool trackChanges = false)
    {
        var game = await _gameRepository.GetSingleAsync(
            specification, trackChanges);
        return AuthorizeForGame(game);
    }
}