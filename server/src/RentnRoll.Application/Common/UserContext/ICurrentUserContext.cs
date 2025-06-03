namespace RentnRoll.Application.Common.UserContext;

public interface ICurrentUserContext
{
    string UserId { get; }
    List<string> Roles { get; }
}