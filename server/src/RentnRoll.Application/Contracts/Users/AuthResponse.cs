namespace RentnRoll.Application.Contracts.Users;

public record AuthResponse(
    string Token,
    UserResponse User
);