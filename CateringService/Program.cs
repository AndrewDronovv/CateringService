using CateringService.Extensions;
using CateringService.Filters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddPersistence();
builder.Services.ApplicationServiceExtensions();

builder.Services.AddApiVesioning();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddPresentationServices();

builder.Services.AddScoped<LoggingActionFilter>();


var app = builder.Build();


app.ConfigurePipeline(builder);
app.Run();

Log.CloseAndFlush();