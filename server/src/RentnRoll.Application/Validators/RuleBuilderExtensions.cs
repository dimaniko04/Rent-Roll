using FluentValidation;

namespace RentnRoll.Application.Validators;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new PhoneValidator<T>());
    }
}