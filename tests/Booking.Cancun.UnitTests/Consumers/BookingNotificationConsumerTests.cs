using Booking.Cancun.Application.Consumers;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Notifications;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.UnitTests.Utils.Seeds;
using Newtonsoft.Json;

namespace Booking.Cancun.UnitTests.Consumers;

public class BookingNotificationConsumerTests
{
    private readonly ILoggerManager _logger;
    private readonly INotificationService _notificationService;
    private readonly BookingNotificationConsumer _bookingNotificationConsumer;

    public BookingNotificationConsumerTests()
    {
        _logger = Substitute.For<ILoggerManager>();
        _notificationService = Substitute.For<INotificationService>();

        _bookingNotificationConsumer = new BookingNotificationConsumer(_logger, _notificationService);
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenRequested_DoNotCallAnyNotification()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingNotificationConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _notificationService.DidNotReceive().SendCancelledNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendBookedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedNotification(Arg.Any<BookingOrderDomain>());
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenDenied_MustCallDeniedNotification()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.Denied();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingNotificationConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _notificationService.DidNotReceive().SendCancelledNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendBookedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.Received(1).SendDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedNotification(Arg.Any<BookingOrderDomain>());
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenBooked_MustCallBookedNotification()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.Booked();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingNotificationConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _notificationService.DidNotReceive().SendCancelledNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.Received(1).SendBookedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedNotification(Arg.Any<BookingOrderDomain>());
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenCancelled_MustCallCancelledNotification()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.Cancelled();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingNotificationConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _notificationService.Received(1).SendCancelledNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendBookedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedNotification(Arg.Any<BookingOrderDomain>());
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenUpdateDenied_MustCallUpdateDeniedNotification()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.UpdateDenied();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingNotificationConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _notificationService.DidNotReceive().SendCancelledNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendBookedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.Received(1).SendUpdatedDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedNotification(Arg.Any<BookingOrderDomain>());
    }

    [Fact]
    public async Task ConsumeMessageAsync_WhenUpdated_MustCallUpdatedNotification()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.Updated();
        var stringMessage = JsonConvert.SerializeObject(bookingDomain);

        // Act
        await _bookingNotificationConsumer.ConsumeMessageAsync(stringMessage, new CancellationToken());

        // Assert
        await _notificationService.DidNotReceive().SendCancelledNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendBookedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.DidNotReceive().SendUpdatedDeniedNotification(Arg.Any<BookingOrderDomain>());
        await _notificationService.Received().SendUpdatedNotification(Arg.Any<BookingOrderDomain>());
    }
}
