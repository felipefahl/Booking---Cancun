using System.Diagnostics.CodeAnalysis;
using Booking.Cancun.Infraestructure.CrossCutting.HangFire;
using Hangfire;
using Hangfire.SqlServer;
using StackExchange.Redis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class HangfireSetup
{
    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RedisConnection");
        var redis = ConnectionMultiplexer.Connect(connectionString);

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(redis)
            .WithJobExpirationTimeout(TimeSpan.FromDays(15)));

        services.AddHangfireServer();
    }

    public static IApplicationBuilder UseHangfireSetup(this IApplicationBuilder app, IConfiguration configuration)
    {
#if DEBUG
        return app.UseHangfireDashboard("/booking-cancun/hangfire-dashboard");
#endif
        return app.UseHangfireDashboard("/booking-cancun/hangfire-dashboard", new DashboardOptions()
        {
            Authorization = new[]
                {
                new HangfireAuthorizationFilter()
                {
                    User = configuration.GetValue<string>("HangfireSettings:UserName"),
                    Pass = configuration.GetValue<string>("HangfireSettings:Password")
                }
            },
            DisplayStorageConnectionString = false,
            DashboardTitle = "Booking Cancun"
        }
        );
    }
}
