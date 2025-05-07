using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CateringService.Filters;

public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items.TryGetValue("StartTime", out var startTimeObj) && startTimeObj is Stopwatch stopwatch)
        {
            stopwatch.Stop();
            _logger.LogInformation($"Контроллер: {context.Controller.GetType().Name}, метод {context.HttpContext.Request.Method} на {context.HttpContext.Request.Path} завершено в {DateTime.UtcNow}, выполнялся {stopwatch.Elapsed.TotalSeconds:F2} сек");
        }
        else
        {
            _logger.LogWarning("Не удалось получить время выполнения запроса.");
        }
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["StartTime"] = Stopwatch.StartNew();
        _logger.LogInformation($"Начато выполнение метода {context.ActionDescriptor.DisplayName}");
    }
}