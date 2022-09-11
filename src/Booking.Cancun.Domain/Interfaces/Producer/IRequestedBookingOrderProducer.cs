using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Producer;

public interface IRequestedBookingOrderProducer
{
    public Task Send(BookingOrderDomain bookingOrderDomain);
}
