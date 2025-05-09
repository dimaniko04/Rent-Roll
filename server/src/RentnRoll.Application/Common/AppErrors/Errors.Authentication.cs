using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.InvalidRequest(
                "Authentication.InvalidCredentials",
                "Invalid email or password.");

        public static Error UserAlreadyExists =>
            Error.InvalidRequest(
                "Authentication.UserAlreadyExists",
                "User with this email already exists.");

        public static Error UserNotFound =>
            Error.NotFound(
                "Authentication.UserNotFound",
                "User with this email does not exist.");

        public static Error InvalidRefreshToken =>
            Error.NotFound(
                "Authentication.InvalidRefreshToken",
                "Invalid refresh token.");

        public static Error InvalidToken =>
            Error.NotFound(
                "Authentication.InvalidToken",
                "Invalid token.");
    }
}