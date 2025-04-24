using Serilog;

namespace RentnRoll.Api.Extensions;

public static class BuilderExtensions
{
    public static void AddSerilog(
        this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration)
        );
    }
}