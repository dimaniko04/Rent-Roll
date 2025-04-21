using RentnRoll.Core.Common;

namespace RentnRoll.Core.Entities;

public class TestEntity : BaseEntity
{
    public string Name { get; init; } = null!;
    public string Hidden { get; init; } = null!;
}