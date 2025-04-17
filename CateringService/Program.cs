using CateringService.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);   
});

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ApplicationServiceExtensions();
builder.Services.AddPersistence();
builder.Services.AddPresentationServices();

var app = builder.Build();

app.ConfigurePipeline();
app.Run();