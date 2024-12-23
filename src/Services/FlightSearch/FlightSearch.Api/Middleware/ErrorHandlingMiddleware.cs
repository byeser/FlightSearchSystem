using FlightSearch.Shared.Exceptions;

namespace FlightSearch.Api.Middleware;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            FlightSearchException ex => (400, new { error = ex.Message, code = ex.ErrorCode }),
            _ => (500, new { error = "An error occurred", code = "INTERNAL_ERROR" })
        };

        response.StatusCode = statusCode;
        await response.WriteAsJsonAsync(message);
    }
}