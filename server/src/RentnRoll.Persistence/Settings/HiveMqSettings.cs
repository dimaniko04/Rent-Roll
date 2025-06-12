namespace RentnRoll.Persistence.Settings;

public class HiveMqSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; } = 8883;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}