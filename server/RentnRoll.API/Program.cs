using RentnRoll.API;
using RentnRoll.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, config) =>
        config.ReadFrom.Configuration(context.Configuration)
    );
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();

    app.MapControllers();
}

await app.RunAsync();