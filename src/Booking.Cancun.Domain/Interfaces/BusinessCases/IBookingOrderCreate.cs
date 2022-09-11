using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Dtos.Response;

namespace Booking.Cancun.Domain.Interfaces.BusinessCases;

public interface IBookingOrderCreate
{
    Task<BookingOrderResponseDTO> CreateBookingOrder(BookingOrderRequestDTO bookingOrderRequest);
}
