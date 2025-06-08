using FluentValidation;

namespace RentnRoll.Application.Validators;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new PhoneValidator<T>());
    }

    public static IRuleBuilderOptions<T, string> IsEnumName<T, TEnum>(
        this IRuleBuilder<T, string> ruleBuilder)
        where TEnum : struct, Enum
    {
        return ruleBuilder.SetValidator(new EnumValidator<T, TEnum>());
    }
}