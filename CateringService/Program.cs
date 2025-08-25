using CateringService.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureSerilog();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddHealthCheckService(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddPersistence();
builder.Services.ApplicationServiceExtensions();
builder.Services.AddApiVesioning();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddPresentationServices();
builder.Services.AddLoggingActionFilter();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.ConfigurePipeline(builder);

app.Logger.LogInformation("Application started successfully.");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}