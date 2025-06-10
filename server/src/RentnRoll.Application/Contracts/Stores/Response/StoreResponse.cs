using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Contracts.Stores.Response;

public record StoreResponse(
    Guid Id,
    string Name,
    string Address,
    DateTime CreatedAt
)
{
    public static StoreResponse FromStore(Store store)
    {
        return new StoreResponse(
            store.Id,
            store.Name,
            store.Address.ToString(),
            store.CreatedAt
        );
    }
}