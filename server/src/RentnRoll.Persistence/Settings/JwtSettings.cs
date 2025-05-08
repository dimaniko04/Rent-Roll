namespace RentnRoll.Persistence.Settings;

public class JwtSettings
{
    public static string SectionName => "JwtSettings";

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int ExpirationInMinutes { get; set; }
}