using Newtonsoft.Json;

namespace Booking.Cancun.Domain.Entities;

public class BookingOrderStayDomain
{
    [JsonProperty]
    public Guid BookingOrderId { get; private set; }

    [JsonProperty]
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
