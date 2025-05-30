using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Games;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Games;

public interface IGameService
{
    Task<Result<GameDetailsResponse>> GetGameDetailsAsync(
        Guid gameId);
    Task<PaginatedResponse<GameResponse>> GetAllGamesAsync(
        GetAllGamesRequest request);
    Task<PaginatedResponse<GameNameResponse>> GetAllGameNamesAsync(
        GetAllGameNamesRequest request);
}