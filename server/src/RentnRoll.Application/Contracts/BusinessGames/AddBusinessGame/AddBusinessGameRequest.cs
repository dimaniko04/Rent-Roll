using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.BusinessGames.AddBusinessGame;

public record AddBusinessGameRequest(
    Guid GameId,
    int Quantity = 1,
    int BasePrice = 0,
    ICollection<string>? Tags = null
)
{
    public BusinessGame ToBusinessGame(
        Guid businessId,
        List<Tag> tags)
    {
        return new BusinessGame
        {
            GameId = GameId,
            Quantity = Quantity,
            BasePrice = BasePrice,
            BusinessId = businessId,
            Tags = tags,
        };
    }
}