using System.Text.Json.Serialization;

namespace Booking.Cancun.Domain.Entities;

public class BookingOrderStayDomain
{
    public Guid BookingOrderId { get; private set; }

    public DateTime Day { get; private set; }

    internal BookingOrderStayDomain(Guid bookingOrderId, DateTime day)
    {
        BookingOrderId = bookingOrderId;

        Day = day;
    }

    [JsonConstructor]
    private BookingOrderStayDomain()
    {
    }
}
