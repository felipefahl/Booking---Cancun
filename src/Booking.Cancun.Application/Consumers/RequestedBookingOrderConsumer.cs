using Booking.Cancun.Application.Producers;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Infraestructure.CrossCutting.Consumers;
using DotNetCore.CAP;

namespace Booking.Cancun.Application.Consumers;

public class RequestedBookingOrderConsumer : KafkaConsumer<BookingOrderDomain>
{
    private readonly IBookingOrderStay _bookingOrderStay;

    public RequestedBookingOrderConsumer(IBookingOrderStay bookingOrderStay) :
        base()
    {
        _bookingOrderStay = bookingOrderStay;
    }

    [CapSubscribe(nameof(RequestedBookingOrderProducer))]
    public override async Task ConsumeMessageAsync(string stringMessage, CancellationToken cancellationToken)
    {
        var message = ConvertMessage(stringMessage);

        await _bookingOrderStay.StayBookingOrder(message);
        return;
    }
}
