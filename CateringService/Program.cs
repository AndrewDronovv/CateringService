using CateringService.Application.Mapping;
using CateringService.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddPersistence();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();