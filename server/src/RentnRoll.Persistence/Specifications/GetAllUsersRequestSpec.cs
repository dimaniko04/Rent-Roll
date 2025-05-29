using RentnRoll.Persistence.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Application.Specifications.Common;

namespace RentnRoll.Persistence.Specifications;

public sealed class GetAllUsersRequestSpec : Specification<User>
{
    public GetAllUsersRequestSpec(GetAllUsersRequest request)
    {
        AddCriteria(u => u.IsDeleted == request.IsDeleted);

        ApplyOrderByDescending(u => u.CreatedAt);
        ApplySorting(request.SortBy);
    }
}