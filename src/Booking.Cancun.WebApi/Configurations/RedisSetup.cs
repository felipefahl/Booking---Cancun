﻿using Booking.Cancun.Domain.Interfaces.Cache;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.Infraestructure.CrossCutting.Cache;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Cancun.WebApi.Configurations;

[ExcludeFromCodeCoverage]
public static class RedisSetup
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RedisConnection");
        var redisConfigurationSection = configuration.GetValue<int>("CacheSettings:ExpirationCacheDefaultSeconds");

        services.AddSingleton<ICacheStore, RedisCacheStore>(sp =>
        {
            return new RedisCacheStore(connectionString,
                                       redisConfigurationSection,
                                       sp.GetRequiredService<ILoggerManager>());
        });
    }
}
