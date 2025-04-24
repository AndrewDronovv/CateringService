using CateringService.Application.Abstractions;
using CateringService.Application.Mapping;
using CateringService.Application.Services;
using CateringService.Application.Validators.Dish;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Repositories;
using CateringService.Persistence;
using CateringService.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace CateringService.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierService, SupplierService>();

        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IDishService, DishService>();

        services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
        services.AddScoped<IMenuCategoryService, MenuCategoryService>();

        services.AddScoped<IUnitOfWorkRepository, UnitOfWork>();
    }

    public static void ApplicationServiceExtensions(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddValidatorsFromAssembly(typeof(DishCreateDtoValidator).Assembly);
        services.AddFluentValidationAutoValidation();
    }

    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
    }
}