using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Jobs;

public interface IRequestedBookingOrderJob
{
    public Task Send(BookingOrderDomain bookingOrderDomain);
}
