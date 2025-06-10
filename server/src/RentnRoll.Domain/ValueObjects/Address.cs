namespace RentnRoll.Domain.ValueObjects;

public record Address(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode)
{
    override public string ToString()
    {
        return $"{Street} {City}, {State}, {Country} {ZipCode}";
    }
};