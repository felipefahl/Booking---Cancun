using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.Infraestructure.CrossCutting.Producers;
using DotNetCore.CAP;

namespace Booking.Cancun.Application.Producers;

public class BookingNotificationProducer : KafkaProducer<BookingOrderDomain>, IBookingNotificationJob
{
    public BookingNotificationProducer(ILoggerManager logger, ICapPublisher kafka)
        : base(logger, kafka, nameof(BookingNotificationProducer))
    {
    }

    public async Task Send(BookingOrderDomain bookingOrderDomain)
    {
        await PublishAsync(bookingOrderDomain);
    }
}
