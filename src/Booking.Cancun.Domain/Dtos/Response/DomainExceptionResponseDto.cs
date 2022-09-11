namespace Booking.Cancun.Domain.Dtos.Response;

public class DomainExceptionResponseDto
{
    public int StatusCode { get; private set; }
    public string? Message { get; private set; }
    public string? TraceId { get; private set; }

    public IDictionary<string, string[]>? Errors { get; private set; }

    public DomainExceptionResponseDto(int statusCode, IDictionary<string, string[]> errors, string? traceId)
    {
        StatusCode = statusCode;
        Errors = errors;
        TraceId = traceId;
    }

    public DomainExceptionResponseDto(int statusCode, string message, string? traceId)
    {
        StatusCode = statusCode;
        Message = message;
        TraceId = traceId;
    }
}
