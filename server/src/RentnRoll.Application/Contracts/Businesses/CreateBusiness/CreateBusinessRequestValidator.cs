using FluentValidation;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Specifications.Businesses;

namespace RentnRoll.Application.Contracts.Businesses.CreateBusiness;

public class CreateBusinessRequestValidator
    : AbstractValidator<CreateBusinessRequest>
{
    public CreateBusinessRequestValidator(
        IUnitOfWork unitOfWork,
        bool skipDuplicateCheck = false)
    {
        var repository = unitOfWork
            .GetRepository<IBusinessRepository>();

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(300)
            .WithMessage("Name must not exceed 300 characters.");

        RuleFor(x => x.Name)
            .MustAsync(async (name, cancellation) =>
            {
                var specification = new BusinessNameSpec(name);
                var exists = await repository
                    .GetSingleAsync(specification);
                return exists == null;
            })
            .When(x => !skipDuplicateCheck)
            .WithMessage("A business with this name already exists.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters.");
    }
}