using CateringService.Extensions;
using CateringService.Filters;
using CateringService.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddScoped<LoggingActionFilter>();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddPersistence();

builder.Services.ApplicationServiceExtensions();

builder.Services.AddApiVesioning();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services.AddControllers(options => options.AddCustomModelBinders());
builder.Services.AddPresentationServices();


var app = builder.Build();

app.ConfigurePipeline();
app.Run();