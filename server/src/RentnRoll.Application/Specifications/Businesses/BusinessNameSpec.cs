using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Specifications.Businesses;

public sealed class BusinessNameSpec : Specification<Business>
{
    public BusinessNameSpec(string name)
    {
        AddCriteria(b => b.Name == name);
    }
}