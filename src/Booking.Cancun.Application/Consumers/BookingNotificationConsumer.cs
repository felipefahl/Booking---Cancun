using Booking.Cancun.Application.Producers;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Enums;
using Booking.Cancun.Domain.Interfaces.Notifications;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.Infraestructure.CrossCutting.Consumers;
using DotNetCore.CAP;

namespace Booking.Cancun.Application.Consumers;

public class BookingNotificationConsumer : KafkaConsumer<BookingOrderDomain>
{
    private readonly ILoggerManager _logger;
    private readonly INotificationService _notificationService;
    public BookingNotificationConsumer(ILoggerManager logger, 
        INotificationService notificationService)
        : base()
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    [CapSubscribe(nameof(BookingNotificationProducer))]
    public override async Task ConsumeMessageAsync(string stringMessage, CancellationToken cancellationToken)
    {
        _logger.LogInfo("BookingNotificationJob");

        var message = ConvertMessage(stringMessage);
        switch (message.Status)
        {
            case EBookingOrderStatus.Cancelled:
                await _notificationService.SendCancelledNotification(message);
                break;
            case EBookingOrderStatus.Booked:
                await _notificationService.SendBookedNotification(message);
                break;
            case EBookingOrderStatus.Denied:
                await _notificationService.SendDeniedNotification(message);
                break;
            case EBookingOrderStatus.UpdateDenied:
                await _notificationService.SendUpdatedDeniedNotification(message);
                break;
            case EBookingOrderStatus.Updated:
                await _notificationService.SendUpdatedNotification(message);
                break;
        }
    }
}
