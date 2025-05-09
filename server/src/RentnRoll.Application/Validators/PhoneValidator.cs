using System.Text.RegularExpressions;

using FluentValidation;
using FluentValidation.Validators;

namespace RentnRoll.Application.Validators;

public class PhoneValidator<T> : PropertyValidator<T, string>
{
    private readonly string _phoneRegexPattern =
        @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    public override string Name => "PhoneValidator";

    public override bool IsValid(
        ValidationContext<T> context,
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return Regex.IsMatch(value, _phoneRegexPattern);
    }

    protected override string GetDefaultMessageTemplate(
        string errorCode)
    {
        return "{PropertyName} must be a valid phone number.";
    }
}