using CateringService.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddPersistence();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();