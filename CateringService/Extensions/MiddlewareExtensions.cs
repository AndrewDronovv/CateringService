using CateringService.Middlewares;

namespace CateringService.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigurePipeline(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}