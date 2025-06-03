using RentnRoll.Application.Contracts.Common;
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
    Task<IEnumerable<Review>> GetGameReviewsAsync(Guid gameId);
    Task AddGameReviewAsync(Review review);
    void UpdateGameReview(Review review);
    Task RemoveGameReviewAsync(Guid gameId, string userIds);
}