namespace Booking.Cancun.Domain.Interfaces.Cache;

public interface ICacheKey<TItem>
{
    string CacheKey { get; }
}
