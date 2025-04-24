using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class TestEntity
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound(
                "TestEntity.NotFound",
                $"TestEntity with ID '{id}' was not found.");
    }
}