using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Contracts.Stores.CreateStore;

public record CreateStoreRequest(
    string Name,
    Address Address
)
{
    public Store ToStore(Guid businessId)
    {
        return new Store
        {
            Name = Name,
            Address = Address,
            BusinessId = businessId,
            CreatedAt = DateTime.UtcNow
        };
    }
};

