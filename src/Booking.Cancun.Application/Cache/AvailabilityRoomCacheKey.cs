using Booking.Cancun.Domain.Interfaces.Cache;

namespace Booking.Cancun.Application.Cache;

public class AvailabilityRoomCacheKey : ICacheKey<AvailabilityRoomCache>
{
    public AvailabilityRoomCacheKey(int roomNumber)
    {
        RoomNumber = roomNumber;
    }

    private int RoomNumber { get; }

    public string CacheKey => $"AvailabilityRoomCache-{RoomNumber}";
}
