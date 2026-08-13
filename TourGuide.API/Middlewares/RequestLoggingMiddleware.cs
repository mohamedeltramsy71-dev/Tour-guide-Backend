namespace TourGuide.API.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var start = DateTime.UtcNow;

        _logger.LogInformation("➡️ {Method} {Path} started at {Time}",
            context.Request.Method,
            context.Request.Path,
            start);

        await _next(context);

        var duration = (DateTime.UtcNow - start).TotalMilliseconds;

        _logger.LogInformation("⬅️ {Method} {Path} → {StatusCode} in {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration);
    }
}