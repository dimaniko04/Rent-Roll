using FluentValidation;

using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Businesses.CreateBusiness;

namespace RentnRoll.Application.Contracts.Businesses.UpdateBusiness;

public class UpdateBusinessRequestValidator
    : AbstractValidator<UpdateBusinessRequest>
{
    public UpdateBusinessRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x)
            .SetValidator(new CreateBusinessRequestValidator(
                unitOfWork,
                skipDuplicateCheck: true));
    }
}