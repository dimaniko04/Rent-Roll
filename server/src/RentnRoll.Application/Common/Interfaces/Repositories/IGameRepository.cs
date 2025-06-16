using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Games.GetAllGames;
using RentnRoll.Application.Contracts.Games.Response;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IGameRepository : IBaseRepository<Game>
{
    Task<PaginatedResponse<GameResponse>> GetPaginatedAsync(
        ISpecification<Game> specification);
    Task<PaginatedResponse<GameNameResponse>> GetGameNamesAsync(
        ISpecification<Game> specification);
    Task<GameDetailsResponse?> GetGameDetailsAsync(
        ISpecification<Game> specification);
    Task<PaginatedResponse<RentableGameResponse>>
        GetRentableGamesAsync(
            GetAllRentableGamesRequest request);
    Task<RentableGameDetailsResponse?>
        GetRentableGameDetailsAsync(
            Guid businessGameId,
            Guid locationId);
}