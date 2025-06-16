using Microsoft.Extensions.Options;

using Quartz;

namespace RentnRoll.Infrastructure.Jobs;

public class RentalOverdueJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(RentalOverdueJob));

        options
            .AddJob<RentalOverdueJob>(builder =>
                builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithIdentity($"{jobKey.Name}-trigger")
                .WithSimpleSchedule(schedule =>
                    schedule
                        .WithInterval(TimeSpan.FromSeconds(30))
                        .RepeatForever()));
    }
}
