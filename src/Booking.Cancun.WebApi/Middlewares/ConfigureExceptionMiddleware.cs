using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Dtos.Response;
using Booking.Cancun.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Booking.Cancun.WebApi.Middlewares;

public class ConfigureExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager _logger;

    public ConfigureExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, params DomainException[] exceptions)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = exceptions
            .SelectMany(x => x.DomainNotifications)
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => new[] { String.Join(", ", x.Select(y => y.Value)) });

        var response = JsonSerializer.Serialize(new DomainExceptionResponseDto(
            context.Response.StatusCode,
            errors,
            context.TraceIdentifier));

        return context.Response.WriteAsync(response);
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(JsonSerializer.Serialize(new DomainExceptionResponseDto(
            context.Response.StatusCode,
            "Server Was Unable To Process The Request" + " - " + exception.Message,
             context.TraceIdentifier
        )));
    }
}
