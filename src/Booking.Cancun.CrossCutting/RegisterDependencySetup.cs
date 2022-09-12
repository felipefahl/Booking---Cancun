using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.CrossCutting;

[ExcludeFromCodeCoverage]
public static class RegisterDependencySetup
{
    public static void AddCrossCuttingDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManagerSerilog>();
    }
}
