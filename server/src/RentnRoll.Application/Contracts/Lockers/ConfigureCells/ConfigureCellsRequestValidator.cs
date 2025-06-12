using FluentValidation;

namespace RentnRoll.Application.Contracts.Lockers.ConfigureCells;

public class ConfigureCellsRequestValidator
    : AbstractValidator<ConfigureCellsRequest>
{
    public ConfigureCellsRequestValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("Device identifier is required.");

        RuleFor(x => x.Pins)
            .NotEmpty()
            .WithMessage("Pins are required.");

        RuleFor(x => x.Pins)
            .Must(pins => pins.Count <= 100)
            .WithMessage("The maximum number of pins is 100.")
            .When(x => x.Pins is not null)
            .Must(pins => pins.All(pin => pin >= 0 && pin <= 255))
            .WithMessage("All pins must be between 0 and 255.")
            .When(x => x.Pins is not null);
    }
}