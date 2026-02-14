using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Teams.ServiceDefaults.Middleware;

/// <summary>
/// Middleware that logs the duration and outcome of each HTTP request.
/// </summary>
public class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString;

        _logger.LogInformation(
            "Request started: {Method} {Path}{QueryString}",
            method, path, queryString);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next(context);
            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var elapsed = stopwatch.Elapsed.TotalMilliseconds;

            if (statusCode >= 500)
            {
                _logger.LogError(
                    "Request completed: {Method} {Path} responded {StatusCode} in {ElapsedMs:F1}ms",
                    method, path, statusCode, elapsed);
            }
            else if (statusCode >= 400)
            {
                _logger.LogWarning(
                    "Request completed: {Method} {Path} responded {StatusCode} in {ElapsedMs:F1}ms",
                    method, path, statusCode, elapsed);
            }
            else
            {
                _logger.LogInformation(
                    "Request completed: {Method} {Path} responded {StatusCode} in {ElapsedMs:F1}ms",
                    method, path, statusCode, elapsed);
            }
        }
        catch (Exception)
        {
            stopwatch.Stop();

            _logger.LogError(
                "Request failed: {Method} {Path} threw exception after {ElapsedMs:F1}ms",
                method, path, stopwatch.Elapsed.TotalMilliseconds);

            throw;
        }
    }
}
