using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Infraestructure.Data.MsSql.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.Infraestructure.Data.MsSql;

[ExcludeFromCodeCoverage]
public static class RegisterDependencySetup
{
    public static void AddDataMsSqlDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IBookingOrderRepository, BookingOrderRepository>();
    }
}
