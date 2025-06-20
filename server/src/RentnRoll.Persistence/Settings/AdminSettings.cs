namespace RentnRoll.Persistence.Settings;

public class AdminSettings
{
    public static string SectionName => "AdminSettings";

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}