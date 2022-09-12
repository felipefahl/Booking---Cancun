using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class CapSetup
{
    public static void AddCapLibrary(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaConnectionString = configuration.GetConnectionString("KafkaConnection");
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddCap(configuration =>
        {
            configuration.UseKafka(options =>
            {
                options.Servers = kafkaConnectionString;
            });
            configuration.UseSqlServer(connectionString);
            configuration.UseDashboard();
        });
    }
}
