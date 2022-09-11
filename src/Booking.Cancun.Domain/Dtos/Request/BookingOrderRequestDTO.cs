namespace Booking.Cancun.Domain.Dtos.Request;

public class BookingOrderRequestDTO
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Email { get; set; }
}
