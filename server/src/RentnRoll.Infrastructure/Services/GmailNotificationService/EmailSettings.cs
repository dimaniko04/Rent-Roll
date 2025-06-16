namespace RentnRoll.Infrastructure.Services.GmailNotificationService;

public class EmailSettings
{
    public string SmtpServer { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public string SenderEmail { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string AppPassword { get; set; } = null!;
}