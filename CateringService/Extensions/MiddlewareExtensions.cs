using Asp.Versioning.ApiExplorer;
using CateringService.Middlewares;

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
        app.UseHttpsRedirection();
        app.UseRequestCulture();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}