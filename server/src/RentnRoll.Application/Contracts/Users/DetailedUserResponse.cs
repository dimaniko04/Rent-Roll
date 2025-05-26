namespace RentnRoll.Application.Contracts.Users;

public record DetailedUserResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Country,
    DateTime? BirthDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt,
    IEnumerable<string> Roles
);