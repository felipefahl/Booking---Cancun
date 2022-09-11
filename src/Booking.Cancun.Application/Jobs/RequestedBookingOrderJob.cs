using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Infraestructure.CrossCutting.Jobs;

namespace Booking.Cancun.Application.Producers;

public class RequestedBookingOrderJob : HangFireJob<BookingOrderDomain>, IRequestedBookingOrderJob
{
    private readonly IBookingOrderStay _bookingOrderStay;
    public RequestedBookingOrderJob(ILoggerManager logger, 
        IBookingOrderStay bookingOrderStay) 
        : base(logger)
    {
        _bookingOrderStay = bookingOrderStay;
    }

    public async Task Send(BookingOrderDomain bookingOrderDomain)
    {
        await Proccess(bookingOrderDomain);
    }

    public async override Task ProccessMessage(BookingOrderDomain message)
    {
        await _bookingOrderStay.StayBookingOrder(message);
    }
}
