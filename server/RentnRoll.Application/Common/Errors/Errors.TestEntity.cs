using RentnRoll.Core.Common;

namespace RentnRoll.Application.Common.Errors;

public static partial class AppErrors
{
    public static class TestEntity
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound(
                "TestEntity.NotFound",
                $"TestEntity with ID '{id}' was not found.");
    }
}