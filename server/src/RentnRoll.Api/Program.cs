using RentnRoll.Api.Extensions;
using RentnRoll.Application;
using RentnRoll.Persistence;
using RentnRoll.Persistence.Identity;

using Serilog;

var CORS_ALLOW_ALL = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(CORS_ALLOW_ALL)
    .AddApplication()
    .AddPersistence(builder.Configuration);

builder.AddSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentnRoll API V1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseHsts();
}

app.UseCors(CORS_ALLOW_ALL);
app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.MapIdentityApi<User>();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

await app.RunAsync();