using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.Games.CreateGame;
using RentnRoll.Application.Contracts.Games.GetAllGames;
using RentnRoll.Application.Contracts.Games.UpdateGame;
using RentnRoll.Application.Services.Games;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Constants;
using RentnRoll.Domain.Entities.Games;

using Serilog;

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
        var authorizeResult = await AuthorizeForGameAsync(gameId);
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
        Log.Debug(gameId.ToString());
        var authorizeResult = await AuthorizeForGameAsync(gameId);
        if (authorizeResult.IsError)
            return Problem(authorizeResult.Errors);

        var game = authorizeResult.Value!;
        var result = await _gameService
            .UpdateGameThumbnailAsync(game, thumbnail);
        return result.Match(Ok, Problem);
    }

    private async Task<Result<Game>> AuthorizeForGameAsync(
        Guid gameId)
    {
        var game = await _gameRepository.GetByIdAsync(gameId);
        Log.Debug(gameId.ToString());
        if (game is null)
            return Errors.Games.NotFound;

        var authorizationResult = _authorizationService
            .AuthorizeAsync(User, game, Policy.CreatorOrAdmin).Result;

        if (!authorizationResult.Succeeded)
            return Errors.User.Unauthorized;

        return game;
    }
}