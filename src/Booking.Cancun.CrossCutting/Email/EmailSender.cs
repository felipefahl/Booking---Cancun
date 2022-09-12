using Booking.Cancun.Domain.Interfaces.Email;
using System.Net;
using System.Net.Mail;

namespace Booking.Cancun.Infraestructure.CrossCutting.Email;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailSender(string host, int port, string username, string password)
    {
        _smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
    }


    public async Task SendEmail(string from, string to, string subject, string body)
    {
        await _smtpClient.SendMailAsync(from, to, subject, body);
    }
}
