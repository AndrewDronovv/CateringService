using Microsoft.AspNetCore.Mvc.Filters;

namespace CateringService.Filters;

public class LoggingActionFilter : Attribute, IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"Получен запрос на {context.ActionDescriptor.DisplayName}");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"Метод {context.ActionDescriptor.DisplayName} завершен");
    }
}