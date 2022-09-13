using Booking.Cancun.Application.Consumers;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.UnitTests.Utils.Seeds;
using Newtonsoft.Json;

namespace Booking.Cancun.UnitTests.Consumers;

public class RequestedBookingOrderConsumerTests
{
    private readonly IBookingOrderStay _bookingOrderStay;
    private readonly RequestedBookingOrderConsumer _requestedBookingOrderConsumer;

    public RequestedBookingOrderConsumerTests()
    {
        _bookingOrderStay = Substitute.For<IBookingOrderStay>();

        _requestedBookingOrderConsumer = new RequestedBookingOrderConsumer(_bookingOrderStay);
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenCalled_MustCallStayBookingOrder()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _requestedBookingOrderConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _bookingOrderStay.Received(1).StayBookingOrder(Arg.Any<BookingOrderDomain>());
    }
}
