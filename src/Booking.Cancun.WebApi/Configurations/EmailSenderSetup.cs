using Booking.Cancun.Domain.Interfaces.Email;
using Booking.Cancun.Infraestructure.CrossCutting.Email;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class EmailSenderSetup
{
    public static void AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("Smtp:Host");
        var port = configuration.GetValue<int>("Smtp:Port");
        var username = configuration.GetValue<string>("Smtp:Username");
        var password = configuration.GetValue<string>("Smtp:Password");

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddSingleton<IEmailSender>(x => new EmailSender(host, port, username, password));
    }
}
