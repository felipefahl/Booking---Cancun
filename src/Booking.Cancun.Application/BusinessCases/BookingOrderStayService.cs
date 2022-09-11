using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Interfaces.Repository;

namespace Booking.Cancun.Application.BusinessCases;

public class BookingOrderStayService : IBookingOrderStay
{
    private readonly ILoggerManager _logger;
    private readonly IBookingOrderRepository _bookingOrderRepository;
    private readonly IBookedBookingOrderJob _bookedBookingOrderJob;
    private readonly IDeniedBookingOrderJob _deniedBookingOrderJob;

    public BookingOrderStayService(ILoggerManager logger,
        IBookingOrderRepository bookingOrderRepository,
        IBookedBookingOrderJob bookedBookingOrderJob,
        IDeniedBookingOrderJob deniedBookingOrderJob)
    {
        _logger = logger;
        _bookingOrderRepository = bookingOrderRepository;
        _bookedBookingOrderJob = bookedBookingOrderJob;
        _deniedBookingOrderJob = deniedBookingOrderJob;
    }

    public async Task StayBookingOrder(BookingOrderDomain bookingOrderDomain)
    {
        _logger.LogInfo("StayBookingOrder");

        if (!await IsBookinDateAvailable(bookingOrderDomain))
        {

            bookingOrderDomain.Denied();
            _bookingOrderRepository.Insert(bookingOrderDomain);

            await _bookingOrderRepository.CommitAsync();

            await _deniedBookingOrderJob.Send(bookingOrderDomain);
            return;
        }

        bookingOrderDomain.Booked();
        _bookingOrderRepository.Insert(bookingOrderDomain);

        bookingOrderDomain.GenerateStays();

        var stays = bookingOrderDomain.Stays;
        _bookingOrderRepository.InsertStay(stays.ToArray());
        await _bookingOrderRepository.CommitAsync();

        await _bookedBookingOrderJob.Send(bookingOrderDomain);
    }

    private async Task<bool> IsBookinDateAvailable(BookingOrderDomain bookingOrderDomain)
    {
        var daysNotAvailables = await _bookingOrderRepository.AllStaysDateByRoomPeriodAsync(bookingOrderDomain.RoomNumber,
            bookingOrderDomain.StartDate,
            bookingOrderDomain.EndDate);

        for (var date = bookingOrderDomain.StartDate.Date; date.Date <= bookingOrderDomain.EndDate.Date; date = date.AddDays(1))
        {
            if (daysNotAvailables.Contains(date))
                return false;
        }

        return true;
    } 
} 
