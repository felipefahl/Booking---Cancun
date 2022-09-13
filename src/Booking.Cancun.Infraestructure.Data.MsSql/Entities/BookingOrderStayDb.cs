using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Infraestructure.Data.MsSql.Entities;

public class BookingOrderStayDb
{
    public Guid BookingOrderId { get; private set; }

    public DateTime Day { get; private set; }

    public BookingOrderDb? BookingOrder { get; private set; }

    protected BookingOrderStayDb()
    {

    }

    public BookingOrderStayDb(BookingOrderStayDomain bookingOrderStayDomain)
    {
        BookingOrderId = bookingOrderStayDomain.BookingOrderId;

        Day = bookingOrderStayDomain.Day;
    }

}
