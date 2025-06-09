using CateringService.Extensions;
using CateringService.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
builder.Services.AddScoped<LoggingActionFilter>();


builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ApplicationServiceExtensions();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddPersistence();
builder.Services.AddPresentationServices();

var app = builder.Build();

//await using (var serviceScope = app.Services.CreateAsyncScope())
//await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>())
//{
//    await dbContext.Database.EnsureCreatedAsync();
//}

app.ConfigurePipeline();
app.Run();

// Не работает эндпоинт для создания MenuCategories;
// Не работает обновление арендатора в Tenants;
// BlockTenant разобраться с передачей параметров и маршрутизацией;