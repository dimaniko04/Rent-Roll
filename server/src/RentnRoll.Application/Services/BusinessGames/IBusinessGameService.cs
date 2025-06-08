using RentnRoll.Application.Contracts.BusinessGames.AddBusinessGame;
using RentnRoll.Application.Contracts.BusinessGames.DeleteBusinessGames;
using RentnRoll.Application.Contracts.BusinessGames.GetAllBusinessGames;
using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Application.Contracts.BusinessGames.UpdateBusinessGame;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.BusinessGames;

public interface IBusinessGameService
{
    Task<PaginatedResponse<BusinessGameResponse>> GetAllBusinessGamesAsync(
        Guid businessId,
        GetAllBusinessGamesRequest request);

    Task<Result<BusinessGameResponse>> GetBusinessGameAsync(
        Guid businessId,
        Guid businessGameId);

    Task<Result<Guid>> AddBusinessGameAsync(
        Guid businessId,
        AddBusinessGameRequest request);

    Task<Result<BusinessGameResponse>> UpdateBusinessGameAsync(
        Guid businessId,
        Guid businessGameId,
        UpdateBusinessGameRequest request);

    Task<Result> DeleteBusinessGameAsync(
        Guid businessId,
        DeleteBusinessGamesRequest request);
}