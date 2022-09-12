namespace Booking.Cancun.Domain.Interfaces.Email;

public interface IEmailSender
{
    Task SendEmail(string from, string to, string subject, string body);
}
