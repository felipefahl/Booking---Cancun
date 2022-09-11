using Booking.Cancun.Application;
using Booking.Cancun.CrossCutting;
using Booking.Cancun.Infraestructure.Data.MsSql;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class RegisterDependencySetup
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {

        services.AddApplicationDependencyInjection();
        services.AddCrossCuttingDependencyInjection();
        services.AddDataMsSqlDependencyInjection();
    }
}
