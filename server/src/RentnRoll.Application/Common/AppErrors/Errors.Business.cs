using RentnRoll.Application.Contracts.Businesses.Response;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Businesses
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Business.AlreadyExists",
                $"Business with name \"{name}\" already exists.");

        public static Error UserAlreadyHasBusiness =>
            Error.InvalidRequest(
                "Business.UserAlreadyHasBusiness",
                "User already has a registered business.");

        public static Error NotFound =>
            Error.NotFound(
                "Business.NotFound",
                "Business does not exist.");

        public static Error Blocked =>
            Error.InvalidRequest(
                "Business.Blocked",
                "Business was blocked by administration.");
    }
}