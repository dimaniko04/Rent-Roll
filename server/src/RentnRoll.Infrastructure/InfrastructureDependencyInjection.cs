using Microsoft.Extensions.DependencyInjection;

using Quartz;

using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Infrastructure.Jobs;
using RentnRoll.Infrastructure.Services.GmailNotificationService;
using RentnRoll.Infrastructure.Services.MqttPublisher;

namespace RentnRoll.Infrastructure;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddBackgroundJobs();
        services.AddServices();

        return services;
    }

    private static void AddBackgroundJobs(
        this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(q =>
        {
            q.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<RentalOverdueJobSetup>();
    }

    private static void AddServices(
        this IServiceCollection services)
    {
        services.Configure<HiveMqSettings>(options =>
        {
            options.Host = Environment
                .GetEnvironmentVariable("MQTT_HOST") ?? "";
            options.Port = int.Parse(Environment
                .GetEnvironmentVariable("MQTT_PORT") ?? "8883");
            options.Username = Environment
                .GetEnvironmentVariable("MQTT_USERNAME") ?? "";
            options.Password = Environment
                .GetEnvironmentVariable("MQTT_PASSWORD") ?? "";
        });


        services.Configure<EmailSettings>(options =>
        {
            options.SenderEmail = Environment
                .GetEnvironmentVariable("GMAIL_SENDER_EMAIL") ?? "";
            options.SenderName = Environment
                .GetEnvironmentVariable("GMAIL_SENDER_NAME") ?? "";
            options.AppPassword = Environment
                .GetEnvironmentVariable("GMAIL_APP_PASSWORD") ?? "";
            options.SmtpServer = Environment
                .GetEnvironmentVariable("GMAIL_SMTP_SERVER") ?? "smtp.gmail.com";
            options.SmtpPort = int.Parse(Environment
                .GetEnvironmentVariable("GMAIL_SMTP_PORT") ?? "587");
        });

        services.AddScoped<IMqttPublisher, HiveMqPublisher>();
        services.AddScoped<INotificationService, GmailNotificationService>();
    }
}