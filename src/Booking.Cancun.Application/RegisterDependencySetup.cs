using Booking.Cancun.Application.BusinessCases;
using Booking.Cancun.Application.Cache;
using Booking.Cancun.Application.Producers;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.Application;

[ExcludeFromCodeCoverage]
public static class RegisterDependencySetup
{
    public static void AddApplicationDependencyInjection(this IServiceCollection services)
    {

        services.AddScoped<IBookingOrderCreate, BookingService>();
        services.AddScoped<IBookingOrderStay, BookingOrderStayService>();

        services.AddScoped<IAvailabilityService, AvailabilityService>();


        services.AddScoped<IRequestedBookingOrderJob, RequestedBookingOrderJob>();
        services.AddScoped<IBookedBookingOrderJob, BookedBookingOrderJob>();
        services.AddScoped<IDeniedBookingOrderJob, DeniedBookingOrderJob>();

        services.AddScoped<IAvailabilityRoomRepository, AvailabilityRoomCacheStore>();
    }
}
