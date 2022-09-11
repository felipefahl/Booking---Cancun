using Booking.Cancun.WebApi.Middlewares;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class MiddlewaresSetup
{
    public static IApplicationBuilder UseCustomException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ConfigureExceptionMiddleware>();
    }
}
