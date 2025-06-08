using FluentValidation;
using FluentValidation.Validators;

namespace RentnRoll.Application.Validators;

public class EnumValidator<T, TEnum> : PropertyValidator<T, string>
    where TEnum : struct, Enum
{
    public override string Name => "EnumValidator";

    public override bool IsValid(
        ValidationContext<T> context,
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return Enum.TryParse<TEnum>(value, out var _);
    }

    protected override string GetDefaultMessageTemplate(
        string errorCode)
    {
        return $"{{PropertyName}} must be one of: {string.Join(", ", Enum.GetNames<TEnum>())}.";
    }
}