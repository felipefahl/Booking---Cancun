namespace Booking.Cancun.Domain.Interfaces.BusinessCases;

public interface IBookingOrderCancel
{
    Task CancelBookingOrder(Guid id);
}
