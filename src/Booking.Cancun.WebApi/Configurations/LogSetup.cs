using Serilog;
using Serilog.Exceptions;
using Serilog.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class LogSetup
{
    public static void UseLog(this IApplicationBuilder app, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.WithProperty("Environment", configuration)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
        .WriteTo.Debug()
        .WriteTo.Console()
        .CreateLogger();
    }
}
