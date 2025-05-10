namespace RentnRoll.Application.Contracts.Users;

public record UpdateUserRequest(
    string Country,
    string LastName,
    string FirstName,
    DateTime BirthDate,
    string Email,
    string PhoneNumber
);