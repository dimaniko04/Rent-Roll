using RentnRoll.Application.Contracts.Games.GetAllGames;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Specifications.Games;

public sealed class GetAllGamesRequestSpec : Specification<Game>
{
    public GetAllGamesRequestSpec(GetAllGamesRequest request)
    {
        AddCriteria(g => g.Age >= request.Age);

        if (request.IsVerified.HasValue)
        {
            AddCriteria(g => g.IsVerified == request.IsVerified.Value);
        }

        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(g => g.Name.Contains(request.Search));
        }

        if (request.MinPlayers != null && request.MinPlayers > 0)
        {
            AddCriteria(g => g.MinPlayers >= request.MinPlayers);
        }
        if (request.MaxPlayers != null &&
            request.MaxPlayers >= (request.MinPlayers ?? 0))
        {
            AddCriteria(g => g.MaxPlayers <= request.MaxPlayers);
        }

        if (request.MinPlayTime != null && request.MinPlayTime > 0)
        {
            AddCriteria(g => g.AveragePlayTime >= request.MinPlayTime);
        }
        if (request.MaxPlayTime != null &&
            request.MaxPlayTime >= (request.MinPlayTime ?? 0))
        {
            AddCriteria(g => g.AveragePlayTime <= request.MaxPlayTime);
        }

        if (request.Genres != null && request.Genres.Any())
        {
            AddCriteria(g => g.Genres.Any(genre =>
                request.Genres.Contains(genre.Name)));
        }
        if (request.Mechanics != null && request.Mechanics.Any())
        {
            AddCriteria(g => g.Mechanics.Any(mechanic =>
                request.Mechanics.Contains(mechanic.Name)));
        }
        if (request.Categories != null && request.Categories.Any())
        {
            AddCriteria(g => g.Categories.Any(category =>
                request.Categories.Contains(category.Name)));
        }

        ApplySorting(request.SortBy);
        ApplyPaging(request.PageNumber, request.PageSize);
    }
}