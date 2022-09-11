namespace Booking.Cancun.Domain.Interfaces.Cache;

public interface ICacheStore
{
    /// <summary>
    /// Add or Replace a value on cache identified by te given key.
    /// </summary>
    /// <typeparam name="TItem">Type of the item to be cached</typeparam>
    /// <param name="item">Item to be cached</param>
    /// <param name="key">Key that represents that cached item</param>
    /// <param name="expirationTime">Time to expire. If <see cref="null"/> it will use the default value set on AppSettings</param>
    Task Add<TItem>(TItem item, ICacheKey<TItem> key, TimeSpan? expirationTime = null);

    /// <summary>
    /// Add or Replace a value on cache identified by te given key.
    /// </summary>
    /// <typeparam name="TItem">Type of the item to be cached</typeparam>
    /// <param name="item">Item to be cached</param>
    /// <param name="key">Key that represents that cached item</param>
    /// <param name="absoluteExpiration">Absolute DateTime to expire. If <see cref="null"/> it will use the <see cref="DateTimeOffset.MaxValue"/></param>
    Task Add<TItem>(TItem item, ICacheKey<TItem> key, DateTime? absoluteExpiration = null);

    /// <summary>
    /// Get an item by it's given CacheKey
    /// </summary>
    /// <typeparam name="TItem">Type of the cached item</typeparam>
    /// <param name="key">Key that identifies the cached item</param>
    /// <returns>The cached instance of the item. Return null if the key is not found.</returns>
    Task<TItem> Get<TItem>(ICacheKey<TItem> key) where TItem : class;

    /// <summary>
    /// Remove an item from cache
    /// </summary>
    /// <typeparam name="TItem">The Type of the item to be removed.</typeparam>
    /// <param name="key">The key that identifies the cached item</param>
    Task Remove<TItem>(ICacheKey<TItem> key);
}
