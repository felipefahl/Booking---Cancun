using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Dtos.Response;

public class BookingOrderResponseDTO
{
    public Guid Id { get; private set; }

    public BookingOrderResponseDTO(BookingOrderDomain bookingOrderDomain)
    {
        Id = bookingOrderDomain.Id;
    }
}
