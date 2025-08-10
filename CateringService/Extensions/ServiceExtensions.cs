using Asp.Versioning;
using CateringService.Application.Abstractions;
using CateringService.Application.Interfaces;
using CateringService.Application.Mapping;
using CateringService.Application.Services;
using CateringService.Application.Validators.Dish;
using CateringService.Domain.Abstractions;
using CateringService.Domain.Interfaces;
using CateringService.Domain.Repositories;
using CateringService.Filters;
using CateringService.ModelBinders.MenuCategories;
using CateringService.ModelBinders.Tenants;
using CateringService.OpenApi;
using CateringService.Persistence;
using CateringService.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantService, TenantService>();

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAddressService, AddressService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();

        services.AddScoped<IUnitOfWorkRepository, UnitOfWork>();

        services.AddScoped<ISlugService, SlugService>();
    }

    public static void AddLoggingActionFilter(this IServiceCollection services)
    {
        services.AddScoped<LoggingActionFilter>();

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<LoggingActionFilter>();
        });
    }

    public static void ApplicationServiceExtensions(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(typeof(DishCreateRequestValidator).Assembly);
        services.AddFluentValidationAutoValidation();
    }

    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new AddMenuCategoryRequestModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new BlockTenantRequestModelBinderProvider());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
        });
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt:AccessToken");
        var key = jwtSettings["Key"] ??
            throw new InvalidOperationException("JWT Key is not configured");
        var issuer = jwtSettings["Issuer"] ??
            throw new InvalidOperationException("JWT Issuer is not configured");
        var audience = jwtSettings["Audience"] ??
            throw new InvalidOperationException("JWT Audience is not configured");

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

    public static void AddApiVesioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(x => x.FullName);

            var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlFilePath);
        });
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();
    }

    public static void AddHealthCheckService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string: DefaultConnection was not found.");

        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("Service is running."), tags: ["live"])
            .AddNpgSql(
                connectionString: connectionString,
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                timeout: TimeSpan.FromSeconds(5),
                tags: ["ready", "db"]
            );
    }
}