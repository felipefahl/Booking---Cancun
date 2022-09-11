using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Interfaces.Cache;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Booking.Cancun.Infraestructure.CrossCutting.Cache;

public class RedisCacheStore : ICacheStore
{
    private readonly ConnectionMultiplexer _conexao;
    private readonly IDatabase _cache;
    private readonly ILoggerManager _logger;

    private readonly int _expirationRedisCacheDefault;

    public RedisCacheStore(string redisServer, int expirationRedisCacheDefault, ILoggerManager logger)
    {
        _conexao = ConnectionMultiplexer.Connect(redisServer);
        _cache = _conexao.GetDatabase();
        _logger = logger;

        _expirationRedisCacheDefault = expirationRedisCacheDefault;
    }

    ///<inheritdoc/>
    public async Task Add<TItem>(TItem item, ICacheKey<TItem> key, TimeSpan? expirationTime = null)
    {
        try
        {
            if (!_conexao.IsConnected)
            {
                return;
            }

            expirationTime = expirationTime ?? new TimeSpan(0, 0, _expirationRedisCacheDefault);

            var stringData = JsonConvert.SerializeObject(item);
            await _cache.StringSetAsync(key.CacheKey, stringData, expirationTime);
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(ex));
            return;
        }
    }

    ///<inheritdoc/>
    public async Task Add<TItem>(TItem item, ICacheKey<TItem> key, DateTime? absoluteExpiration = null)
    {
        try
        {
            if (!_conexao.IsConnected)
            {
                return;
            }

            absoluteExpiration = absoluteExpiration ?? DateTime.Now.AddSeconds(_expirationRedisCacheDefault);

            var expirationInterval = absoluteExpiration - DateTime.Now;

            var stringData = JsonConvert.SerializeObject(item);
            await _cache.StringSetAsync(key.CacheKey, stringData, expirationInterval);
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(ex));
            return;
        }
    }

    ///<inheritdoc/>
    public async Task<TItem> Get<TItem>(ICacheKey<TItem> key) where TItem : class
    {
        try
        {
            if (!_conexao.IsConnected)
            {
                return default(TItem);
            }

            var stringData = (await _cache.StringGetAsync(key.CacheKey)).ToString();

            if (string.IsNullOrEmpty(stringData))
            {
                return default(TItem);
            }

            return JsonConvert.DeserializeObject<TItem>(stringData);
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(ex));
            return default(TItem);
        }
    }

    ///<inheritdoc/>
    public async Task Remove<TItem>(ICacheKey<TItem> key)
    {
        try
        {
            if (!_conexao.IsConnected)
            {
                return;
            }

            await _cache.StringGetDeleteAsync(key.CacheKey);
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(ex));
            return;
        }
    }
}
