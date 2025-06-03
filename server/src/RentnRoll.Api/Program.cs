using RentnRoll.Api.Extensions;
using RentnRoll.Application;
using RentnRoll.Application.Common.Options;
using RentnRoll.Persistence;
using RentnRoll.Persistence.Extensions;

using Serilog;

var CORS_ALLOW_ALL = "AllowAll";

var builder = WebApplication.CreateBuilder(args);

var uploadsSubFolder = "uploads";
var uploadsPath = Path.Combine(
    builder.Environment.WebRootPath ?? "wwwroot",
    uploadsSubFolder);

builder.Services.Configure<FileStorageOptions>(options =>
{
    options.UploadRoot = uploadsPath;
    options.UploadSubFolder = uploadsSubFolder;
});

builder.Services
    .AddPresentation(CORS_ALLOW_ALL)
    .AddApplication()
    .AddPersistence(builder.Configuration);

builder.AddSerilog();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
    await app.SeedDataAsync();

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

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();