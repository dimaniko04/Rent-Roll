using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error Unauthorized =>
            Error.Forbidden(
                "Authentication.Unauthorized",
                "You are not authorized to perform this action.");

        public static Error AlreadyExists =>
            Error.InvalidRequest(
                "Authentication.UserAlreadyExists",
                "User with this email already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Authentication.UserNotFound",
                "User with this email is blocked or does not exist.");

        public static Error AdminCannotBeBlocked =>
            Error.Forbidden(
                "Authentication.AdminCannotBeBlocked",
                "Admin cannot be blocked.");
    }
}