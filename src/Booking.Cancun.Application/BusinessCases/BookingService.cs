using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Exceptions;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.Domain.Notifications;

namespace Booking.Cancun.Application.BusinessCases;

public class BookingService : IBookingOrderStay, IBookingOrderUpdate, IBookingOrderCancel
{
    private readonly ILoggerManager _logger;
    private readonly IBookingOrderRepository _bookingOrderRepository;
    private readonly IBookingOrderAvailabilityJob _bookedBookingOrderJob;
    private readonly IBookingNotificationJob _bookingNotificationJob;

    public BookingService(ILoggerManager logger,
        IBookingOrderRepository bookingOrderRepository,
        IBookingOrderAvailabilityJob bookedBookingOrderJob,
        IBookingNotificationJob bookingNotificationJob)
    {
        _logger = logger;
        _bookingOrderRepository = bookingOrderRepository;
        _bookedBookingOrderJob = bookedBookingOrderJob;
        _bookingNotificationJob = bookingNotificationJob;
    }

    public async Task CancelBookingOrder(Guid id)
    {
        _logger.LogInfo("CancelBookingOrder");

        var bookingOrderDomain = await _bookingOrderRepository.FindAsync(id);

        if (bookingOrderDomain == null)
            throw new DomainException(new DomainNotification("Id", "Booking Id not found"));

        bookingOrderDomain.Cancelled();
        _bookingOrderRepository.Update(bookingOrderDomain);
        _bookingOrderRepository.DeleteBookingStay(bookingOrderDomain.Id);

        await _bookingOrderRepository.CommitAsync();

        await _bookedBookingOrderJob.Send(bookingOrderDomain);
        await _bookingNotificationJob.Send(bookingOrderDomain);
    }

    public async Task StayBookingOrder(BookingOrderDomain bookingOrderDomain)
    {
        _logger.LogInfo("StayBookingOrder");

        if (!await IsBookinDateAvailable(bookingOrderDomain.RoomNumber,
            bookingOrderDomain.Id,
            bookingOrderDomain.StartDate,
            bookingOrderDomain.EndDate))
        {

            bookingOrderDomain.Denied();
            _bookingOrderRepository.Insert(bookingOrderDomain);

            await _bookingOrderRepository.CommitAsync();

            await _bookingNotificationJob.Send(bookingOrderDomain);
            return;
        }

        bookingOrderDomain.Booked();
        _bookingOrderRepository.Insert(bookingOrderDomain);

        bookingOrderDomain.GenerateStays();

        var stays = bookingOrderDomain.Stays;
        _bookingOrderRepository.InsertStay(stays.ToArray());
        await _bookingOrderRepository.CommitAsync();

        await _bookedBookingOrderJob.Send(bookingOrderDomain);
        await _bookingNotificationJob.Send(bookingOrderDomain);
    }

    public async Task UpdateBookingOrder(Guid id, BookingOrderRequestDTO bookingOrderRequest)
    {
        _logger.LogInfo("UpdateBookingOrder");

        var bookingOrderDomain = await _bookingOrderRepository.FindAsync(id);

        if (bookingOrderDomain == null)
            throw new DomainException(new DomainNotification("Id", "Booking Id not found"));

        if (!await IsBookinDateAvailable(bookingOrderDomain.RoomNumber, 
            bookingOrderDomain.Id,
            bookingOrderRequest.StartDate,
            bookingOrderRequest.EndDate))
        {
            bookingOrderDomain.UpdateDenied();
            _bookingOrderRepository.Update(bookingOrderDomain);

            await _bookingOrderRepository.CommitAsync();

            await _bookingNotificationJob.Send(bookingOrderDomain);
            return;
        }

        bookingOrderDomain.Update(bookingOrderRequest);
        bookingOrderDomain.Updated();

        _bookingOrderRepository.DeleteBookingStay(bookingOrderDomain.Id);

        bookingOrderDomain.GenerateStays();
        var stays = bookingOrderDomain.Stays;
        _bookingOrderRepository.InsertStay(stays.ToArray());

        _bookingOrderRepository.Update(bookingOrderDomain);

        await _bookingOrderRepository.CommitAsync();

        await _bookedBookingOrderJob.Send(bookingOrderDomain);
        await _bookingNotificationJob.Send(bookingOrderDomain);
    }

    private async Task<bool> IsBookinDateAvailable(int roomNumber, Guid id, DateTime startDate, DateTime endDate)
    {
        var available = await _bookingOrderRepository.PeriodAvailableAsync(roomNumber,
            id,
            startDate,
            endDate);

        return available;
    } 
} 
