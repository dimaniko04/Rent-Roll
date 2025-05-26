using RentnRoll.Application.Common.Interfaces.Contracts;

namespace RentnRoll.Application.Contracts.Users;

public record GetAllUsersRequest(
    int PageNumber = 1,
    int PageSize = 30
) : IPaginationRequest;
