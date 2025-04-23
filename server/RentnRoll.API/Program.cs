using RentnRoll.API.Extensions;
using RentnRoll.Application;
using Serilog;

var CORS_ALLOW_ALL = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(CORS_ALLOW_ALL)
    .AddApplication();

builder.AddSerilog();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentnRoll API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors(CORS_ALLOW_ALL);
app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.MapControllers();


await app.RunAsync();