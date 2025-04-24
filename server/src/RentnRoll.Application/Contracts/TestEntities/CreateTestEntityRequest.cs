using RentnRoll.Domain.Entities;

namespace RentnRoll.Application.Contracts.TestEntities;

public record CreateTestEntityRequest(
    string Name,
    string Secret)
{
    public TestEntity ToDomain()
    {
        return new TestEntity
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Hidden = Secret
        };
    }
};