using RentnRoll.Domain.Common;

namespace RentnRoll.Domain.Entities;

public class TestEntity : Entity
{
    public string Name { get; init; } = null!;
    public string Hidden { get; init; } = null!;
}