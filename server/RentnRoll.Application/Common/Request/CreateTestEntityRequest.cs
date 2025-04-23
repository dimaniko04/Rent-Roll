using RentnRoll.Core.Entities;

namespace RentnRoll.Application.Common.Request;

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