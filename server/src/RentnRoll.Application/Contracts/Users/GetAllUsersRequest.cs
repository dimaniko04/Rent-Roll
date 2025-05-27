using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Application.Contracts.Users;

public record GetAllUsersRequest(
    bool IsDeleted = false
) : QueryParams;
