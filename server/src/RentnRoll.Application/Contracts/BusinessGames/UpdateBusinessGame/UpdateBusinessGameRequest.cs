using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.BusinessGames.UpdateBusinessGame;

public record UpdateBusinessGameRequest(
    int Quantity = 1,
    int BasePrice = 0,
    ICollection<string>? Tags = null
)
{
    public BusinessGame UpdateBusinessGame(
        BusinessGame businessGame,
        List<Tag> tags)
    {
        businessGame.Quantity = Quantity;
        businessGame.BasePrice = BasePrice;
        businessGame.Tags = tags;

        return businessGame;
    }
}