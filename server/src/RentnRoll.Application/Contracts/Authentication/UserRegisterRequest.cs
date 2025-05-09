namespace RentnRoll.Application.Contracts.Authentication;

public record UserRegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    DateTime BirthDate,
    string Country,
    string Password,
    string PhoneNumber
);