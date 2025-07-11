namespace RentnRoll.Domain.Common;

public sealed record Error(
    string Code,
    string Message,
    string? Details,
    ErrorType Type = ErrorType.Unknown)
{
    public static readonly Error None = new(string.Empty, string.Empty, string.Empty);

    public static Error NotFound(
        string code = "NotFound",
        string message = "Requested resource not found.",
        string? details = null
        ) => new(code, message, details, ErrorType.NotFound);

    public static Error Forbidden(
        string code = "Forbidden",
        string message = "Requested resource is forbidden.",
        string? details = null
        ) => new(code, message, details, ErrorType.Forbidden);

    public static Error InvalidRequest(
        string code = "InvalidRequest",
        string message = "Invalid request.",
        string? details = null
        ) => new(code, message, details, ErrorType.InvalidRequest);

    public static Error Unsupported(
        string code = "Unsupported",
        string message = "Unsupported media type.",
        string? details = null
        ) => new(code, message, details, ErrorType.Unsupported);

    public static Error Unauthorized(
        string code = "Unauthorized",
        string message = "Authentication required.",
        string? details = null
        ) => new(code, message, details, ErrorType.Unauthorized);

    public static Error Validation(
        string code = "Validation",
        string message = "A validation error has occurred.",
        string? details = null
        ) => new(code, message, details, ErrorType.Validation);
}