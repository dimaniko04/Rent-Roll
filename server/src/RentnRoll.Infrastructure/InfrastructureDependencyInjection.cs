using Microsoft.Extensions.DependencyInjection;

using Quartz;

using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Infrastructure.Jobs;
using RentnRoll.Infrastructure.Services.MqttPublisher;

namespace RentnRoll.Infrastructure;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddBackgroundJobs();
        services.AddScoped<IMqttPublisher, HiveMqPublisher>();

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
}