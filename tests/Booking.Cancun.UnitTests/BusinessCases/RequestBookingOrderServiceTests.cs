using Booking.Cancun.Application.BusinessCases;
using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Dtos.Response;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Logging;

namespace Booking.Cancun.UnitTests.BusinessCases;

public class RequestBookingOrderServiceTests
{
    private readonly ILoggerManager _logger;
    private readonly IRequestedBookingOrderJob _requestedBookingOrderProducer;

    private RequestBookingOrderService _requestBookingOrderService;

    public RequestBookingOrderServiceTests()
    {
        _logger = Substitute.For<ILoggerManager>();
        _requestedBookingOrderProducer = Substitute.For<IRequestedBookingOrderJob>();

        _requestBookingOrderService = new RequestBookingOrderService(_logger, _requestedBookingOrderProducer);
    }

    [Fact]
    public async Task CreateBookingOrder_WhenSuccess_MustCallBookingOrderProducer()
    {
        // Arrange
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(10),
            EndDate = DateTime.Today.AddDays(10),
        };

        // Act
        var result = await _requestBookingOrderService.CreateBookingOrder(dto);

        // Assert
        await _requestedBookingOrderProducer.Received(1).Send(Arg.Any<BookingOrderDomain>());

        Assert.IsType<BookingOrderResponseDTO>(result);
    }
}
