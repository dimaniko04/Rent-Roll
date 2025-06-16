
using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Domain.Entities.Rentals;

namespace RentnRoll.Infrastructure.Services.GmailNotificationService;

public class GmailNotificationService : INotificationService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<GmailNotificationService> _logger;

    public GmailNotificationService(
        IOptions<EmailSettings> options,
        ILogger<GmailNotificationService> logger)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task SendOverdueEmailToUserAsync(
        string userEmail,
        string userFullName,
        Rental rental)
    {
        var subject = $"📦 Rental Overdue: {rental.Game.Name}";
        var body = $"Dear {userFullName},\n\n" +
                   $"Your rental for \"{rental.Game.Name}\" is overdue since {rental.EndDate:d}.\n" +
                   $"Please return it as soon as possible to avoid penalties.\n\nThank you.";

        await SendEmailAsync(userEmail, subject, body);
    }

    public async Task SendOverdueEmailToOwnerAsync(
        string userEmail,
        string userFullName,
        string ownerEmail,
        string ownerFullName,
        Rental rental)
    {
        var subject = $"⚠️ Game Overdue Alert: {rental.Game.Name}";
        var body = $"Dear {ownerFullName},\n\n" +
                   $"User {userFullName} has not returned the game \"{rental.Game.Name}\".\n" +
                   $"It was due on {rental.EndDate:d}. Please follow up if needed.\n\nRegards.";

        await SendEmailAsync(ownerEmail, subject, body);
    }

    private async Task SendEmailAsync(
        string toEmail,
        string subject,
        string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _settings.SenderName, _settings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain") { Text = body };

        try
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _settings.SmtpServer,
                _settings.SmtpPort,
                SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                _settings.SenderEmail,
                _settings.AppPassword);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
        }
    }
}
