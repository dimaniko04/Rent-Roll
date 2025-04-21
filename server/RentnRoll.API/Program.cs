using RentnRoll.Application;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddApplication();
}

var app = builder.Build();
{
    app.MapControllers();
}

await app.RunAsync();