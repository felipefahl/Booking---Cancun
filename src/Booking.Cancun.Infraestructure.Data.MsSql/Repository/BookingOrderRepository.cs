using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Infraestructure.Data.MsSql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Cancun.Infraestructure.Data.MsSql.Repository
{
    public class BookingOrderRepository : IBookingOrderRepository
    {
        private readonly BookingContext _context;

        public BookingOrderRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DateTime>> AllStaysDateByRoomPeriodAsync(int roomNumber, DateTime beginPeriod, DateTime endPeriod)
        {
            return await _context.BookingOrderStays
                .Include(x => x.BookingOrder)
                .Where(x => x.BookingOrder.RoomNumber == roomNumber)
                .Where(x => x.Day >= beginPeriod.Date)
                .Where(x => x.Day <= endPeriod.Date)
                .Select(x => x.Day)
                .ToArrayAsync();
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.CommitAsync();
        }

        public void Insert(params BookingOrderDomain[] orders)
        {
            var ordersDb = orders.Select(x => new BookingOrderDb(x));
            _context.BookingOrders.AddRange(ordersDb);
        }

        public void InsertStay(params BookingOrderStayDomain[] stays)
        {
            var orderStaysDb = stays.Select(x => new BookingOrderStayDb(x));
            _context.BookingOrderStays.AddRange(orderStaysDb);
        }

        public void Update(params BookingOrderDomain[] orders)
        {
            throw new NotImplementedException();
        }
    }
}
