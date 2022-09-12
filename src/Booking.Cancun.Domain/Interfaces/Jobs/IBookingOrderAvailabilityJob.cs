using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Jobs;

public interface IBookingOrderAvailabilityJob
{
    Task Send(BookingOrderDomain bookingOrderDomain);
}
