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
using RentnRoll.Application.Contracts.Games.GetAllGames;
using Microsoft.Extensions.Logging;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Domain.Entities.Lockers.Enums;
using RentnRoll.Domain.Enums;

namespace RentnRoll.Persistence.Repositories;

public class GameRepository : BaseRepository<Game>, IGameRepository
{
    private readonly ILogger<GameRepository> _logger;

    public GameRepository(
        RentnRollDbContext context,
        ILogger<GameRepository> logger) : base(context)
    {
        _logger = logger;
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
                g.IsVerified
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

    public async Task<PaginatedResponse<RentableGameResponse>>
        GetRentableGamesAsync(
            GetAllRentableGamesRequest request)
    {
        _logger.LogInformation(string.Join(",", request.Genres ?? []));

        var query = _context.Database
            .SqlQuery<RentableGameResponse>(@$"
            SELECT 
                U.Id,
                U.GameId,
                BusinessGameId,
                LocationId, 
                LocationType,
                Name, 
                U.Description,
                U.ThumbnailUrl,
                U.PublishedAt,
                U.IsVerified,
                Price,
                U.Address,
                TimeUnit
            FROM
            (
            SELECT 
                    sa.Id AS Id,
                    g.Id AS GameId,
                    bg.Id AS BusinessGameId,
                    g.Name,
                    g.Description,
                    g.ThumbnailUrl,
                    g.PublishedAt,
                    g.IsVerified,
                    s.Id as LocationId,
                    s.Address_Street +
                        s.Address_City +
                        ',' +
                        s.Address_State +
                        ',' +
                        s.Address_Country +
                        s.Address_ZipCode AS Address,
                    ppi.Price, 
                    pp.TimeUnit, 
                    'GameStore' AS LocationType 
                FROM Game g
                INNER JOIN BusinessGame bg 
                    ON bg.GameId = g.Id
                INNER JOIN Store s 
                    ON s.BusinessId = bg.BusinessId
                INNER JOIN StoreAsset sa 
                    ON sa.BusinessGameId = bg.Id
                    AND sa.StoreId = s.Id
                INNER JOIN PricingPolicy pp 
                    ON s.PolicyId = pp.Id
                INNER JOIN PricingPolicyItem ppi
                    ON ppi.GameId = bg.Id
                    AND ppi.PolicyId = s.PolicyId
                WHERE sa.Quantity > 0
            UNION
            SELECT
                    c.Id,
                    g.Id,
                    bg.Id AS BusinessGameId,
                    g.Name,
                    g.Description,
                    g.ThumbnailUrl,
                    g.PublishedAt,
                    g.IsVerified,
                    l.Id as LocationId,
                    l.Address_Street +
                        l.Address_City +
                        ',' +
                        l.Address_State +
                        ',' +
                        l.Address_Country +
                        l.Address_ZipCode,
                    ppi.Price, 
                    pp.TimeUnit,  
                    'Locker' 
                FROM Game g
                INNER JOIN BusinessGame bg 
                    ON bg.GameId = g.Id
                INNER JOIN Cell c 
                    ON c.BusinessGameId = bg.Id
                INNER JOIN Locker l
                    ON c.LockerId = l.Id
                INNER JOIN LockerPricingPolicies lpp 
                    ON l.Id = lpp.LockerId
                INNER JOIN PricingPolicy pp 
                    ON lpp.PricingPoliciesId  = pp.Id
                INNER JOIN PricingPolicyItem ppi
                    ON ppi.GameId = bg.Id
                    AND ppi.PolicyId = lpp.PricingPoliciesId
                WHERE c.Status = 'Available'
            ) U")
            .Join(
                _context.Set<Game>(),
                u => u.GameId,
                g => g.Id,
                (u, game) => new { u, game }
            );

        if (request.LocationType.HasValue)
        {
            query = query
                .Where(j => j.u.LocationType == request.LocationType.Value.ToString());
        }

        if (!string.IsNullOrEmpty(request.Country))
        {
            query = query
                .Where(j => j.u.Address.Contains(request.Country));
        }

        if (request.MinPrice != null && request.MinPrice > 0)
        {
            query = query
                .Where(j => j.u.Price >= request.MinPrice);
        }

        if (request.MaxPrice != null && request.MaxPrice >= (request.MinPrice ?? 0))
        {
            query = query
                .Where(j => j.u.Price <= request.MaxPrice);
        }

        if (request.TimeUnit is not null)
        {
            query = query
                .Where(j => j.u.TimeUnit == request.TimeUnit);
        }

        if (request.Age > 0)
        {
            query = query
                .Where(j => j.game.Age >= request.Age);
        }

        if (request.IsVerified.HasValue)
        {
            query = query
                .Where(j => j.game.IsVerified == request.IsVerified.Value);
        }

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query
                .Where(j => j.game.Name.Contains(request.Search));
        }

        if (request.MinPlayers != null && request.MinPlayers > 0)
        {
            query = query
                .Where(j => j.game.MinPlayers >= request.MinPlayers);
        }
        if (request.MaxPlayers != null &&
            request.MaxPlayers >= (request.MinPlayers ?? 0))
        {
            query = query
                .Where(j => j.game.MaxPlayers <= request.MaxPlayers);
        }

        if (request.MinPlayTime != null && request.MinPlayTime > 0)
        {
            query = query
                .Where(j => j.game.AveragePlayTime >= request.MinPlayTime);
        }
        if (request.MaxPlayTime != null &&
            request.MaxPlayTime >= (request.MinPlayTime ?? 0))
        {
            query = query
                .Where(j => j.game.AveragePlayTime <= request.MaxPlayTime);
        }

        if (request.Genres is not null && request.Genres.Any())
        {
            query = query
                .Where(g => g.game.Genres
                    .Any(genre => request.Genres
                        .Contains(genre.Name)));
        }

        if (request.Mechanics != null && request.Mechanics.Any())
        {
            query = query
                .Where(g => g.game.Mechanics
                    .Any(mechanic => request.Mechanics
                        .Contains(mechanic.Name)));
        }

        if (request.Categories != null && request.Categories.Any())
        {
            query = query
                .Where(g => g.game.Categories
                    .Any(category => request.Categories
                        .Contains(category.Name)));
        }

        return await query
            .Select(j => j.u)
            .OrderBy(u => u.Name)
            .ToPaginatedResponse(
                request.PageNumber,
                request.PageSize);
    }

    public async Task<RentableGameDetailsResponse?>
        GetRentableGameDetailsAsync(Guid id)
    {
        var cellGame = await _context
            .Set<Cell>()
            .Where(c => c.Id == id &&
                c.Status == CellStatus.Available)
            .FirstOrDefaultAsync();

        if (cellGame is not null)
        {
            return await GetRentableGameDetailsFromCellAsync(
                cellGame);
        }

        var storeAssetGame = await _context
            .Set<StoreAsset>()
            .Where(sa => sa.Id == id &&
                sa.Quantity > 0)
            .FirstOrDefaultAsync();

        if (storeAssetGame is not null)
        {
            return await GetRentableGameDetailsFromStoreAsync(
                storeAssetGame);
        }

        return null;
    }

    private Task<RentableGameDetailsResponse?>
        GetRentableGameDetailsFromCellAsync(Cell cell)
    {
        return _context
            .Set<Cell>()
            .AsNoTracking()
            .Where(c => c.Id == cell.Id)
            .Include(c => c.BusinessGame!)
            .ThenInclude(bg => bg.Game)
            .ThenInclude(g => g.Images)
            .Where(c => c.BusinessGame != null)
            .Include(c => c.Locker)
            .ThenInclude(l => l.PricingPolicies)
            .ThenInclude(pp => pp.Items)
            .Select(c =>
                new
                {
                    c,
                    l = c.Locker,
                    g = c.BusinessGame!.Game,
                    pp = c.Locker.PricingPolicies
                        .Select(pp => new RentableGamePrice(
                            pp.UnitCount,
                            pp.TimeUnit.ToString(),
                            pp.Items.FirstOrDefault(ppi =>
                                ppi.GameId == c.BusinessGameId
                            )!.Price
                        ))
                })
            .Select(j => new RentableGameDetailsResponse(
                j.c.Id,
                j.c.BusinessGameId!.Value,
                j.g.Name,
                j.g.Description,
                j.g.ThumbnailUrl,
                j.g.PublishedAt,
                j.g.MinPlayers,
                j.g.MaxPlayers,
                j.g.Age,
                j.g.AveragePlayTime,
                j.g.ComplexityScore,
                j.g.IsVerified,
                j.g.Genres.Select(g => g.Name),
                j.g.Categories.Select(c => c.Name),
                j.g.Mechanics.Select(m => m.Name),
                j.g.Images.Select(i => i.Url),
                j.pp,
                j.l.Name,
                LocationType.Locker.ToString(),
                j.l.Address.ToString()
            ))
            .FirstOrDefaultAsync();
    }


    private Task<RentableGameDetailsResponse?>
        GetRentableGameDetailsFromStoreAsync(StoreAsset storeAsset)
    {
        return _context
            .Set<StoreAsset>()
            .AsNoTracking()
            .Where(a => a.Id == storeAsset.Id)
            .Include(a => a.BusinessGame!)
            .ThenInclude(bg => bg.Game)
            .Where(a => a.BusinessGame != null)
            .Include(a => a.Store)
            .ThenInclude(s => s.Policy)
            .ThenInclude(pp => pp!.Items)
            .Select(a =>
                new
                {
                    a,
                    s = a.Store,
                    g = a.BusinessGame!.Game,
                    pp = new[] {
                    new RentableGamePrice(
                        a.Store.Policy!.UnitCount,
                        a.Store.Policy.TimeUnit.ToString(),
                        a.Store.Policy.Items.FirstOrDefault(ppi =>
                            ppi.GameId == a.BusinessGameId
                        )!.Price
                    )}
                })
            .Select(j => new RentableGameDetailsResponse(
                j.a.Id,
                j.a.BusinessGameId,
                j.g.Name,
                j.g.Description,
                j.g.ThumbnailUrl,
                j.g.PublishedAt,
                j.g.MinPlayers,
                j.g.MaxPlayers,
                j.g.Age,
                j.g.AveragePlayTime,
                j.g.ComplexityScore,
                j.g.IsVerified,
                j.g.Genres.Select(g => g.Name),
                j.g.Categories.Select(a => a.Name),
                j.g.Mechanics.Select(m => m.Name),
                j.g.Images.Select(i => i.Url),
                j.pp,
                j.s.Name,
                LocationType.GameStore.ToString(),
                j.s.Address.ToString()
            ))
            .FirstOrDefaultAsync();
    }
}