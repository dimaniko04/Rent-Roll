using RentnRoll.Core.Entities;

namespace RentnRoll.Application.Common.Responses;

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