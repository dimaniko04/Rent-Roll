using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Categories
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Categories.CategoryAlreadyExists",
                $"Category with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Categories.CategoryNotFound",
                "Category does not exist.");
    }
}