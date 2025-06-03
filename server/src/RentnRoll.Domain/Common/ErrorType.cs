namespace RentnRoll.Domain.Common;

public enum ErrorType
{
    Unknown,
    NotFound,
    Validation,
    Forbidden,
    InvalidRequest,
    Unauthorized,
    Unsupported,
}