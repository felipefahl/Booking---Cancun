using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Repository;

public interface IBookingOrderRepository : IUnitOfWork
{
    Task<BookingOrderDomain> FindAsync(Guid id);
    void Insert(params BookingOrderDomain[] orders);
    void InsertStay(params BookingOrderStayDomain[] stays);
    void Update(params BookingOrderDomain[] orders);

    void DeleteBookingStay(Guid bookingOrderId);

    Task<IEnumerable<DateTime>> AllStaysDateByRoomPeriodAsync(int roomNumber, DateTime beginPeriod, DateTime endPeriod);
    Task<bool> PeriodAvailableAsync(int roomNumber, Guid id, DateTime beginPeriod, DateTime endPeriod);
}
