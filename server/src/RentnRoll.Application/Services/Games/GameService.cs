
using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Games;
using RentnRoll.Application.Specifications.Games;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Games;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = unitOfWork
            .GetRepository<IGameRepository>();
    }

    public async Task<PaginatedResponse<GameNameResponse>> GetAllGameNamesAsync(
        GetAllGameNamesRequest request)
    {
        var specification = new GameSearchSpec(request);

        var gameNames = await _gameRepository
            .GetGameNamesAsync(specification);

        return gameNames;
    }

    public async Task<PaginatedResponse<GameResponse>> GetAllGamesAsync(
        GetAllGamesRequest request)
    {
        var specification = new GameSearchSpec(request);

        var games = await _gameRepository
            .GetPaginatedAsync(specification);

        return games;
    }

    public async Task<Result<GameDetailsResponse>> GetGameDetailsAsync(
        Guid gameId)
    {
        var specification = new GameDetailsSpec(gameId);

        var game = await _gameRepository
            .GetGameDetailsAsync(specification);

        if (game == null)
            return Errors.Games.NotFound;

        return game;
    }
}