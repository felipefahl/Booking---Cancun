using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Repository;

public interface IBookingOrderRepository : IUnitOfWork
{
    void Insert(params BookingOrderDomain[] orders);
    void InsertStay(params BookingOrderStayDomain[] stays);
    void Update(params BookingOrderDomain[] orders);

    Task<IEnumerable<DateTime>> AllStaysDateByRoomPeriodAsync(int roomNumber, DateTime beginPeriod, DateTime endPeriod);
}
