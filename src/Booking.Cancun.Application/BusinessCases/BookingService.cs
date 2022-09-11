using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Dtos.Response;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;

namespace Booking.Cancun.Application.BusinessCases;

public class BookingService : IBookingOrderCreate
{
    private readonly ILoggerManager _logger;
    private readonly IRequestedBookingOrderJob _requestedBookingOrderProducer;

    public BookingService(ILoggerManager logger,
        IRequestedBookingOrderJob requestedBookingOrderProducer)
    {
        _logger = logger;
        _requestedBookingOrderProducer = requestedBookingOrderProducer;
    }

    public async Task<BookingOrderResponseDTO> CreateBookingOrder(BookingOrderRequestDTO bookingOrderRequest)
    {
        _logger.LogInfo("CreateBookingOrder");
        var bookingOrder = new BookingOrderDomain(bookingOrderRequest);
        await _requestedBookingOrderProducer.Send(bookingOrder);
        return new BookingOrderResponseDTO(bookingOrder);
    }
} 
