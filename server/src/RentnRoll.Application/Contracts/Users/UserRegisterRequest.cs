namespace RentnRoll.Application.Contracts.Users;

public record UserRegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    DateTime BirthDate,
    string Country,
    string Password,
    string PhoneNumber
);