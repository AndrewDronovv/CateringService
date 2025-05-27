using CateringService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CateringService.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _requestDelegate;
    public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandling exception.");
            var problem = GetProblemDetails(context, exception);
            context.Response.StatusCode = problem.Status.Value;
            context.Response.ContentType = "application/problem+json";
            var json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
    }
    private static int GetHttpStatusCode(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError,
        };
    }

    private static string GetProblemTitle(Exception exception)
    {
        return exception switch
        {
            NotFoundException => exception.Message,
            _ => "Внутренняя ошибка сервера.",
        };
    }

    private ProblemDetails GetProblemDetails(HttpContext context, Exception exception)
    {
        var problem = new ProblemDetails()
        {
            Status = GetHttpStatusCode(exception),
            Title = GetProblemTitle(exception),
            Detail = exception.Message,
            Instance = context.Request.Path
        };
        return problem;
    }
}

public static class ErrorHandlingMiddlewareExtension
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}