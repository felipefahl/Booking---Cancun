using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Jobs;

public interface IBookingNotificationJob
{
    Task Send(BookingOrderDomain bookingOrderDomain);
}
