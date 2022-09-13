using Booking.Cancun.Application.BusinessCases;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Email;
using Booking.Cancun.UnitTests.Utils.Seeds;

namespace Booking.Cancun.UnitTests.BusinessCases;

public class NotificationServiceTests
{
    private readonly IEmailSender _emailSeter;

    private const string fromMail = "no-reply@booking-cancun.com";
    private string subject = string.Empty;
    private string body = string.Empty;
    private string toEmail = string.Empty;
    private BookingOrderDomain _bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();

    private NotificationService _notificationService;

    public void SetBookedNotification()
    {
        subject = "Booking Confirmed at Cancun";
        toEmail = _bookingDomain.Email;
        body = @$"Congratilations, your booking is confirmed at cancun between 
                    {_bookingDomain.StartDate.ToString("yyyy-MM-dd")} and {_bookingDomain.EndDate.ToString("yyyy-MM-dd")}";
    }

    public void SetUpdatedNotification()
    {
        subject = "Booking Update Confirmed at Cancun";
        toEmail = _bookingDomain.Email;
        body = @$"Congratilations, your booking update is confirmed at cancun between 
                    {_bookingDomain.StartDate.ToString("yyyy-MM-dd")} and {_bookingDomain.EndDate.ToString("yyyy-MM-dd")}";
    }

    public void SetDeniedNotification()
    {
        subject = "Booking Not Confirmed at Cancun";
        toEmail = _bookingDomain.Email;
        body = @$"Sorry, but your booking date has already booked at cancun by another client";
    }

    public void SetCancelledNotification()
    {
        subject = "Booking Cancelled at Cancun";
        toEmail = _bookingDomain.Email;
        body = @$"Your booking date has cancelled";
    }

    public void SetUpdatedDeniedNotification()
    {
        subject = "Booking Update Not Confirmed at Cancun";
        toEmail = _bookingDomain.Email;
        body = @$"Sorry, but your booking update date has already booked at cancun by another client, your original booking is maintained";
    }

    public NotificationServiceTests()
    {
        _emailSeter = Substitute.For<IEmailSender>();

        _notificationService = new NotificationService(_emailSeter);
    }

    [Fact]
    public async Task SendBookedNotification_WhenSuccess_MustCallSendEmail()
    {
        // Arrange
        SetBookedNotification();

        // Act
        await _notificationService.SendBookedNotification(_bookingDomain);

        // Assert
        await _emailSeter.Received(1).SendEmail(fromMail, toEmail, subject, body);
    }

    [Fact]
    public async Task SendUpdatedNotification_WhenSuccess_MustCallSendEmail()
    {
        // Arrange
        SetUpdatedNotification();

        // Act
        await _notificationService.SendUpdatedNotification(_bookingDomain);

        // Assert
        await _emailSeter.Received(1).SendEmail(fromMail, toEmail, subject, body);
    }

    [Fact]
    public async Task SendDeniedNotification_WhenSuccess_MustCallSendEmail()
    {
        // Arrange
        SetDeniedNotification();

        // Act
        await _notificationService.SendDeniedNotification(_bookingDomain);

        // Assert
        await _emailSeter.Received(1).SendEmail(fromMail, toEmail, subject, body);
    }

    [Fact]
    public async Task SendCancelledNotification_WhenSuccess_MustCallSendEmail()
    {
        // Arrange
        SetCancelledNotification();

        // Act
        await _notificationService.SendCancelledNotification(_bookingDomain);

        // Assert
        await _emailSeter.Received(1).SendEmail(fromMail, toEmail, subject, body);
    }

    [Fact]
    public async Task SendUpdatedDeniedNotification_WhenSuccess_MustCallSendEmail()
    {
        // Arrange
        SetUpdatedDeniedNotification();

        // Act
        await _notificationService.SendUpdatedDeniedNotification(_bookingDomain);

        // Assert
        await _emailSeter.Received(1).SendEmail(fromMail, toEmail, subject, body);
    }

}
