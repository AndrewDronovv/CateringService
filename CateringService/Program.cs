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
builder.Services.AddControllers(options => options.AddCustomModelBinders());
builder.Services.AddPresentationServices();

var app = builder.Build();

app.ConfigurePipeline();
app.Run();


// TODO: BlockTenant разобраться с передачей параметров и маршрутизацией;
// Рассмотреть метод контроллера UpdateAddressAsync;
// Вопрос про Tenant как дальше его связать с другими таблицами в бд;
// Вопрос про HasActiveOrdersAsync, возможно его сделать в OrderRepository?
// Вопрос про сущность Order и ее свойсва;
// Тест на создание блюда;