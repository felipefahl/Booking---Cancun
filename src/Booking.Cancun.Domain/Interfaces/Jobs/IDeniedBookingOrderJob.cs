using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Jobs;

public interface IDeniedBookingOrderJob
{
    public Task Send(BookingOrderDomain bookingOrderDomain);
}
