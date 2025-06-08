using RentnRoll.Application.Contracts.BusinessGames.GetAllBusinessGames;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Application.Specifications.BusinessGames;

public sealed class GetAllBusinessGamesRequestSpec
    : Specification<BusinessGame>
{
    public GetAllBusinessGamesRequestSpec(
        Guid businessId,
        GetAllBusinessGamesRequest request)
    {
        AddInclude(bg => bg.Game);
        AddInclude(bg => bg.Tags);

        AddCriteria(bg => bg.BusinessId == businessId);
        AddCriteria(bg => bg.Game.Age >= request.Age);

        if (request.ExcludeIds != null && request.ExcludeIds.Any())
        {
            AddCriteria(bg => !request.ExcludeIds.Contains(bg.Id));
        }

        if (request.isVerified.HasValue)
        {
            AddCriteria(bg => bg.Game.IsVerified == request.isVerified.Value);
        }

        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(bg => bg.Game.Name.Contains(request.Search));
        }

        if (request.MinPlayers != null && request.MinPlayers > 0)
        {
            AddCriteria(bg => bg.Game.MinPlayers >= request.MinPlayers);
        }
        if (request.MaxPlayers != null &&
            request.MaxPlayers >= (request.MinPlayers ?? 0))
        {
            AddCriteria(bg => bg.Game.MaxPlayers <= request.MaxPlayers);
        }

        if (request.MinPlayTime != null && request.MinPlayTime > 0)
        {
            AddCriteria(bg => bg.Game.AveragePlayTime >= request.MinPlayTime);
        }
        if (request.MaxPlayTime != null &&
            request.MaxPlayTime >= (request.MinPlayTime ?? 0))
        {
            AddCriteria(bg => bg.Game.AveragePlayTime <= request.MaxPlayTime);
        }

        if (request.Genres != null && request.Genres.Any())
        {
            AddCriteria(bg => bg.Game.Genres.Any(genre =>
                request.Genres.Contains(genre.Name)));
        }
        if (request.Mechanics != null && request.Mechanics.Any())
        {
            AddCriteria(bg => bg.Game.Mechanics.Any(mechanic =>
                request.Mechanics.Contains(mechanic.Name)));
        }
        if (request.Categories != null && request.Categories.Any())
        {
            AddCriteria(bg => bg.Game.Categories.Any(category =>
                request.Categories.Contains(category.Name)));
        }

        ApplySorting(request.SortBy);
        ApplyPaging(request.PageNumber, request.PageSize);
    }
}