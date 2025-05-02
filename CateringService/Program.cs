using CateringService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ApplicationServiceExtensions();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddPersistence();
builder.Services.AddPresentationServices();

var app = builder.Build();

app.ConfigurePipeline();
app.Run();