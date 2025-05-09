namespace RentnRoll.Application.Contracts.Users;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    UserResponse User
);