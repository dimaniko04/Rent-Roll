namespace RentnRoll.Application.Contracts.Users;

public record UserResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Country,
    DateTime? BirthDate
);