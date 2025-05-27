using System.Linq.Expressions;

using RentnRoll.Persistence.Identity;
using RentnRoll.Application.Contracts.Users;
using RentnRoll.Persistence.Specifications.Common;

namespace RentnRoll.Persistence.Specifications;

public sealed class GetAllUsersRequestSpec : Specification<User>
{
    public GetAllUsersRequestSpec(GetAllUsersRequest request)
    {
        ApplyPagination(request.PageNumber, request.PageSize);

        ApplyOrderBy(user => user.LastName + " " + user.FirstName);
    }

    public override Expression<Func<User, bool>> Criteria => user => true;
}