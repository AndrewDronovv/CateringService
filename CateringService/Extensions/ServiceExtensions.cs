using System.Text;
using CateringService.Application.Abstractions;
using CateringService.Application.Mapping;
using CateringService.Application.Services;
using CateringService.Application.Validators.Dish;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Repositories;
using CateringService.ModelBinders.MenuCategories;
using CateringService.ModelBinders.Tenants;
using CateringService.Persistence;
using CateringService.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

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

    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection string: DefaultConnection was not found.");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information));
    }

    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierService, SupplierService>();

        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IDishService, DishService>();

        services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
        services.AddScoped<IMenuCategoryService, MenuCategoryService>();

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierService, SupplierService>();

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantService, TenantService>();

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAddressService, AddressService>();

        services.AddScoped<IUnitOfWorkRepository, UnitOfWork>();

        services.AddScoped<ISlugService, SlugService>();
    }

    public static void AddCustomModelBinders(this MvcOptions options)
    {
        options.ModelBinderProviders.Insert(0, new AddMenuCategoryRequestModelBinderProvider());
        options.ModelBinderProviders.Insert(0, new BlockTenantRequestModelBinderProvider());
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

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt:AccessToken");
        var key = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
                };
            });
    }
}