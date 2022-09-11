using Booking.Cancun.Infraestructure.Data.MsSql;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class DatabaseSetup
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingContext>(options => options
#if DEBUG
            .UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.ClearProviders();
                builder.AddSerilog();
            }))
            .EnableSensitiveDataLogging()
#endif
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    sqlServerOptions.MigrationsAssembly("Booking.Cancun.Infraestructure.Data.MsSql");
                }));
    }

    public static void UpdateDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.EnvironmentName != "Test")
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScope != null)
            {
                using var context = serviceScope.ServiceProvider.GetRequiredService<BookingContext>();
                context.Database.Migrate();
            }
        }
    }
}
