using Booking.Cancun.Application.Consumers;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.UnitTests.Utils.Seeds;
using Newtonsoft.Json;

namespace Booking.Cancun.UnitTests.Consumers;

public class BookingOrderAvailabilityConsumerTests
{
    private readonly ILoggerManager _logger;
    private readonly IAvailabilityService _availabilityService;

    private readonly BookingOrderAvailabilityConsumer _bookingOrderAvailabilityConsumer;

    public BookingOrderAvailabilityConsumerTests()
    {
        _logger = Substitute.For<ILoggerManager>();
        _availabilityService = Substitute.For<IAvailabilityService>();

        _bookingOrderAvailabilityConsumer = new BookingOrderAvailabilityConsumer(_logger, _availabilityService);
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenCalled_MustCallStayBookingOrder()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.GenerateStays();
        bookingDomain.GenerateStays();

        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingOrderAvailabilityConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _availabilityService.Received(1).SetAvailabilityStays(Arg.Any<IList<AvailabilityRoomDomain>>());
    }
}
