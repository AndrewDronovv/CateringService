﻿using System.Globalization;

namespace CateringService.Middlewares;

public class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureQuery = context.Request.Query["culture"];
        if (!string.IsNullOrWhiteSpace(cultureQuery))
        {
            var culture = new CultureInfo(cultureQuery);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        await _next(context);
    }
}

public static class RequestCultureMiddlewareExtensions
{
    public static void UseRequestCulture(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestCultureMiddleware>();
    }
}