using RentnRoll.Persistence.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Persistence.Specifications.Common;

namespace RentnRoll.Persistence.Specifications;

public sealed class GetAllUsersRequestSpec : Specification<User>
{
    public GetAllUsersRequestSpec(GetAllUsersRequest request)
    {
        ApplyPagination(request.PageNumber, request.PageSize);

        ApplyOrderByDescending(u => u.CreatedAt);
    }
}