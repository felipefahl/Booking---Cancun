using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Infraestructure.CrossCutting.Jobs;

namespace Booking.Cancun.Application.Producers;

public class DeniedBookingOrderJob : HangFireJob<BookingOrderDomain>, IDeniedBookingOrderJob
{
    public DeniedBookingOrderJob(ILoggerManager logger) 
        : base(logger)
    {
    }

    public async Task Send(BookingOrderDomain bookingOrderDomain)
    {
        await Proccess(bookingOrderDomain);
    }

    public async override Task ProccessMessage(BookingOrderDomain message)
    {
        await Task.CompletedTask;
    }
}
