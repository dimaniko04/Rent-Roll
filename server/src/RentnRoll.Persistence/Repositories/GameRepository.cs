using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Specifications;
using RentnRoll.Persistence.Extensions;
using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Application.Contracts.Games.Response;

namespace RentnRoll.Persistence.Repositories;

public class GameRepository : BaseRepository<Game>, IGameRepository
{
    public GameRepository(RentnRollDbContext context) : base(context)
    {
    }

    public Task<PaginatedResponse<GameResponse>> GetPaginatedAsync(
        ISpecification<Game> specification)
    {
        var query = SpecificationEvaluator
            .GetQuery(_dbSet.AsQueryable(), specification);

        return query
            .AsNoTracking()
            .Select(g => new GameResponse(
                g.Id,
                g.Name,
                g.Description,
                g.ThumbnailUrl,
                g.PublishedAt,
                g.IsVerified,
                g.Reviews.Average(r => r.Rating)
            ))
            .ToPaginatedResponse(
                specification.PageNumber,
                specification.PageSize);
    }

    public Task<PaginatedResponse<GameNameResponse>> GetGameNamesAsync(
        ISpecification<Game> specification)
    {
        var query = SpecificationEvaluator
            .GetQuery(_dbSet.AsQueryable(), specification);

        return query
            .AsNoTracking()
            .Select(g => new GameNameResponse(g.Id, g.Name))
            .ToPaginatedResponse(
                specification.PageNumber,
                specification.PageSize);
    }

    public Task<GameDetailsResponse?> GetGameDetailsAsync(
        ISpecification<Game> specification)
    {
        var query = SpecificationEvaluator
            .GetQuery(_dbSet.AsQueryable(), specification);

        return query
            .AsNoTracking()
            .Select(g => new GameDetailsResponse(
                g.Id,
                g.Name,
                g.Description,
                g.ThumbnailUrl,
                g.PublishedAt,
                g.MinPlayers,
                g.MaxPlayers,
                g.Age,
                g.AveragePlayTime,
                g.ComplexityScore,
                g.IsVerified,
                g.VerifiedByUserId,
                g.CreatedByUserId,
                g.Genres.Select(g =>
                    new GenreResponse(g.Id, g.Name)),
                g.Categories.Select(c =>
                    new CategoryResponse(c.Id, c.Name)),
                g.Mechanics.Select(m =>
                    new MechanicResponse(m.Id, m.Name)),
                g.Images.Select(i => i.Url)
            ))
            .FirstOrDefaultAsync();
    }

    public async Task AddGameReviewAsync(Review review)
    {
        await _context.Set<Review>().AddAsync(review);
    }

    public async Task<IEnumerable<Review>> GetGameReviewsAsync(Guid gameId)
    {
        return await _dbSet
            .Where(g => g.Id == gameId)
            .SelectMany(g => g.Reviews)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task RemoveGameReviewAsync(Guid gameId, string userIds)
    {
        await _context.Set<Review>()
            .Where(r => r.GameId == gameId && r.UserId == userIds)
            .ExecuteDeleteAsync();
    }

    public void UpdateGameReview(Review review)
    {
        _context.Set<Review>().Update(review);
    }
}