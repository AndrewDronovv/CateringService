using Asp.Versioning.ApiExplorer;
using CateringService.HealthChecks;
using CateringService.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace CateringService.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigurePipeline(this WebApplication app)
    {
        app.UseErrorHandling();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
                foreach (ApiVersionDescription description in descriptions)
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpper();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live"),
            ResponseWriter = HealthCheckResponseWriter.WriteLivenessResponse
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = HealthCheckResponseWriter.WriteDetailedResponse
        });

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteDetailedResponse
        });

        app.MapHealthChecks("/health/simple", new HealthCheckOptions
        {
            ResponseWriter = HealthCheckResponseWriter.WriteSimpleResponse
        });

        app.UseHttpsRedirection();
        app.UseRequestCulture();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}