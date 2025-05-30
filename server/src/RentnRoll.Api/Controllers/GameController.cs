using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Games;
using RentnRoll.Application.Services.Games;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/games")]
[Authorize]
public class GameController : ApiController
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
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
}