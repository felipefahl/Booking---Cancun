using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Enums;

namespace Booking.Cancun.Infraestructure.Data.MsSql.Entities;

public class BookingOrderDb
{
    public Guid Id { get; private set; }
    public DateTime BookedAt { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int RoomNumber { get; private set; }
    public string? Email { get; private set; }
    public EBookingOrderStatus Status { get; set; }


    public IList<BookingOrderStayDb> Stays { get; private set; }
    protected BookingOrderDb()
    {

    }
    public BookingOrderDb(BookingOrderDomain bookingOrderDomain)
    {
        Id = bookingOrderDomain.Id;
        BookedAt = DateTime.Now;
        StartDate = bookingOrderDomain.StartDate;
        EndDate = bookingOrderDomain.EndDate;
        RoomNumber = bookingOrderDomain.RoomNumber; 
        Email = bookingOrderDomain.Email;
        Status = bookingOrderDomain.Status;

        Stays = new List<BookingOrderStayDb>();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        var compareTo = obj as BookingOrderDb;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode() * 907 + Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}
