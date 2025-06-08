using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.BusinessGames.AddBusinessGame;
using RentnRoll.Application.Contracts.BusinessGames.DeleteBusinessGames;
using RentnRoll.Application.Contracts.BusinessGames.GetAllBusinessGames;
using RentnRoll.Application.Contracts.BusinessGames.UpdateBusinessGame;
using RentnRoll.Application.Services.BusinessGames;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/businesses/{businessId:guid}/games")]
[Authorize]
public class BusinessGameController : ApiController
{
    private readonly IBusinessGameService _businessGameService;

    public BusinessGameController(
        IBusinessGameService businessGameService)
    {
        _businessGameService = businessGameService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGames(
        Guid businessId,
        [FromQuery] GetAllBusinessGamesRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _businessGameService
            .GetAllBusinessGamesAsync(businessId, request);

        return Ok(result);
    }

    [HttpGet("{gameId:guid}", Name = nameof(GetGameById))]
    public async Task<IActionResult> GetGameById(
        Guid businessId, Guid gameId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _businessGameService
            .GetBusinessGameAsync(businessId, gameId);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> AddGame(
        Guid businessId,
        [FromBody] AddBusinessGameRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _businessGameService
            .AddBusinessGameAsync(businessId, request);

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtRoute(
            nameof(GetGameById),
            new { businessId, gameId = result.Value },
            new { gameId = result.Value });
    }

    [HttpPut("{gameId:guid}")]
    public async Task<IActionResult> UpdateGame(
        Guid businessId,
        Guid gameId,
        [FromBody] UpdateBusinessGameRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _businessGameService
            .UpdateBusinessGameAsync(businessId, gameId, request);

        return result.Match(Ok, Problem);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteGame(
        Guid businessId,
        DeleteBusinessGamesRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _businessGameService
            .DeleteBusinessGameAsync(businessId, request);

        return result.Match(Ok, Problem);
    }
}