using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.BusinessCases;

public interface IBookingOrderStay
{
    Task StayBookingOrder(BookingOrderDomain bookingOrderDomain);
}
