using RentnRoll.Domain.Entities;

namespace RentnRoll.Application.Contracts.TestEntities;

public record TestEntityResponse(
    Guid Id,
    string Name)
{
    public static TestEntityResponse FromDomain(TestEntity entity)
    {
        return new TestEntityResponse(
            entity.Id,
            entity.Name);
    }
};