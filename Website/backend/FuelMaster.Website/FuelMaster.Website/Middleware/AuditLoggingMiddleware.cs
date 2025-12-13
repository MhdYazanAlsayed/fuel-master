using System.Diagnostics;
using System.Text;

namespace FuelMaster.Website.Middleware;

public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditLoggingMiddleware> _logger;

    public AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var path = context.Request.Path.Value ?? string.Empty;

        // Only log sensitive operations
        var sensitiveOperations = new[] { "/api/tenants", "/api/subscription", "/api/billing", "/api/admin" };
        var shouldLog = sensitiveOperations.Any(op => path.Contains(op, StringComparison.OrdinalIgnoreCase));

        if (shouldLog)
        {
            var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var method = context.Request.Method;
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            _logger.LogInformation(
                "Audit: User {UserId} performed {Method} on {Path} from {IpAddress}",
                userId, method, path, ipAddress);
        }

        await _next(context);

        stopwatch.Stop();
        if (shouldLog)
        {
            _logger.LogInformation(
                "Request {Path} completed in {ElapsedMilliseconds}ms with status {StatusCode}",
                path, stopwatch.ElapsedMilliseconds, context.Response.StatusCode);
        }
    }
}

