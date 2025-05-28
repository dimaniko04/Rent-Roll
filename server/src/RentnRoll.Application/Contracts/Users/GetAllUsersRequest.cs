using RentnRoll.Application.Contracts.Common;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Application.Contracts.Users;

public record GetAllUsersRequest(
    bool IsDeleted = false,
    string Role = ""
) : QueryParams;
