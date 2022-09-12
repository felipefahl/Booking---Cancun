using Booking.Cancun.Application.BusinessCases;
using Booking.Cancun.Application.Cache;
using Booking.Cancun.Application.Consumers;
using Booking.Cancun.Application.Producers;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Interfaces.Notifications;
using Booking.Cancun.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.Application;

[ExcludeFromCodeCoverage]
public static class RegisterDependencySetup
{
    public static void AddApplicationDependencyInjection(this IServiceCollection services)
    {

        services.AddScoped<IBookingOrderCreate, RequestBookingOrderService>();
        services.AddScoped<IBookingOrderCancel, BookingService>();
        services.AddScoped<IBookingOrderUpdate, BookingService>();
        services.AddScoped<IBookingOrderStay, BookingService>();

        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IBookingNotificationJob, BookingNotificationProducer>();
        services.AddScoped<IBookingOrderAvailabilityJob, BookingOrderAvailabilityProducer>();
        services.AddScoped<IRequestedBookingOrderJob, RequestedBookingOrderProducer>();

        services.AddTransient<RequestedBookingOrderConsumer>();
        services.AddTransient<BookingOrderAvailabilityConsumer>();
        services.AddTransient<BookingNotificationConsumer>();

        services.AddScoped<IAvailabilityRoomRepository, AvailabilityRoomCacheStore>();
    }
}
