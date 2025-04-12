using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//Domain - модели, интерфейсы репозиториев и сервисов а также shared объекты
//Application - Реализации сервисов, хендлеров для медиатра, валидаторов
//Persistant - сущности базы данных, маппинги, миграции
//WebApi - слой презентации - в частности API