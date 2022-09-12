using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Email;
using Booking.Cancun.Domain.Interfaces.Notifications;

namespace Booking.Cancun.Application.BusinessCases;

public class NotificationService : INotificationService
{
    private readonly IEmailSender _emailSender;
    private const string fromMail = "no-reply@booking-cancun.com";

    public NotificationService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendBookedNotification(BookingOrderDomain bookingOrderDomain) {
        var subject = "Booking Confirmed at Cancun";
        var toEmail = bookingOrderDomain.Email;
        var body = @$"Congratilations, your booking is confirmed at cancun between 
                    {bookingOrderDomain.StartDate.ToString("yyyy-MM-dd")} and {bookingOrderDomain.EndDate.ToString("yyyy-MM-dd")}";

        await _emailSender.SendEmail(fromMail, toEmail, subject, body);
    }

    public async Task SendUpdatedNotification(BookingOrderDomain bookingOrderDomain)
    {
        var subject = "Booking Update Confirmed at Cancun";
        var toEmail = bookingOrderDomain.Email;
        var body = @$"Congratilations, your booking update is confirmed at cancun between 
                    {bookingOrderDomain.StartDate.ToString("yyyy-MM-dd")} and {bookingOrderDomain.EndDate.ToString("yyyy-MM-dd")}";

        await _emailSender.SendEmail(fromMail, toEmail, subject, body);
    }

    public async Task SendDeniedNotification(BookingOrderDomain bookingOrderDomain)
    {
        var subject = "Booking Not Confirmed at Cancun";
        var toEmail = bookingOrderDomain.Email;
        var body = @$"Sorry, but your booking date has already booked at cancun by another client";

        await _emailSender.SendEmail(fromMail, toEmail, subject, body);
    }

    public async Task SendCancelledNotification(BookingOrderDomain bookingOrderDomain)
    {
        var subject = "Booking Cancelled at Cancun";
        var toEmail = bookingOrderDomain.Email;
        var body = @$"Your booking date has cancelled";

        await _emailSender.SendEmail(fromMail, toEmail, subject, body);
    }

    public async Task SendUpdatedDeniedNotification(BookingOrderDomain bookingOrderDomain)
    {
        var subject = "Booking Update Not Confirmed at Cancun";
        var toEmail = bookingOrderDomain.Email;
        var body = @$"Sorry, but your booking update date has already booked at cancun by another client, your original booking is maintained";

        await _emailSender.SendEmail(fromMail, toEmail, subject, body);
    }
}
