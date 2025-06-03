using Microsoft.AspNetCore.Http;

using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Games.CreateGame;
using RentnRoll.Application.Contracts.Games.GetAllGames;
using RentnRoll.Application.Contracts.Games.ReplaceGameImages;
using RentnRoll.Application.Contracts.Games.Response;
using RentnRoll.Application.Contracts.Games.UpdateGame;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Services.Games;

public interface IGameService
{
    Task<Result<GameDetailsResponse>> GetGameDetailsAsync(
        Guid gameId);
    Task<PaginatedResponse<GameResponse>> GetAllGamesAsync(
        GetAllGamesRequest request);
    Task<PaginatedResponse<GameNameResponse>> GetAllGameNamesAsync(
        GetAllGameNamesRequest request);
    Task<Result<GameResponse>> CreateGameAsync(
        CreateGameRequest request);
    Task<Result<GameDetailsResponse>> UpdateGameAsync(
        Game game,
        UpdateGameRequest request);
    Task<Result> DeleteGameAsync(Guid gameId);

    Task<Result<string>> UpdateGameThumbnailAsync(
        Game game,
        IFormFile file);

    Task<Result<List<string>>> AddGameImagesAsync(
        Game game,
        List<IFormFile> files);
    Task<Result<List<string>>> ReplaceGameImagesAsync(
        Game game,
        ReplaceGameImagesRequest request);
    Task<Result> DeleteGameImagesAsync(
        Game game,
        List<string> imagePath);
}