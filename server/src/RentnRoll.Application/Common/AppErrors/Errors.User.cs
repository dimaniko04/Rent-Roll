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

        public static Error Blocked =>
            Error.Forbidden(
                "Authentication.UserBlocked",
                "User is blocked.");

        public static Error AlreadyExists =>
            Error.InvalidRequest(
                "Authentication.UserAlreadyExists",
                "User with this email already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Authentication.UserNotFound",
                "User does not exist.");

        public static Error AdminCannotBeBlocked =>
            Error.Forbidden(
                "Authentication.AdminCannotBeBlocked",
                "Admin cannot be blocked.");
    }
}