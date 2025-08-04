using CateringService.Extensions;
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

builder.Services.AddLoggingActionFilter();


var app = builder.Build();

app.ConfigurePipeline();
app.Run();

Log.CloseAndFlush();