using Booking.Cancun.Domain.Dtos.Request;

namespace Booking.Cancun.Domain.Interfaces.BusinessCases;

public interface IBookingOrderUpdate
{
    Task UpdateBookingOrder(Guid id, BookingOrderRequestDTO bookingOrderRequest);
}
