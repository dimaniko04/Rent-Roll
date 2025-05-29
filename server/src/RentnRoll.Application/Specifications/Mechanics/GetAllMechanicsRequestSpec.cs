using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Specifications.Mechanics;

public sealed class GetAllMechanicsRequestSpec : Specification<Mechanic>
{
    public GetAllMechanicsRequestSpec(GetAllMechanicsRequest request)
    {
        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(c => c.Name.Contains(request.Search));
        }

        ApplySorting(request.SortBy);
    }
}