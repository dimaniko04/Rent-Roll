using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Tags
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Tag.AlreadyExists",
                $"Tag with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.InvalidRequest(
                "Tag.NotFound",
                "Tag does not exist.");
    }
}