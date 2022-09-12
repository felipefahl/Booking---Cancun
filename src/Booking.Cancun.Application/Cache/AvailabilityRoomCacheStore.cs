using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Cache;
using Booking.Cancun.Domain.Interfaces.Repository;

namespace Booking.Cancun.Application.Cache;

public class AvailabilityRoomCacheStore : IAvailabilityRoomRepository
{
    private readonly ICacheStore _cacheStore;

    public AvailabilityRoomCacheStore(ICacheStore cacheStore)
    {
        _cacheStore = cacheStore;
    }

    public async Task<IEnumerable<AvailabilityRoomDomain>> AllByRoomAsync(int roomNumber)
    {
        var cached = await _cacheStore.Get(new AvailabilityRoomCacheKey(roomNumber));

        if (cached != null)
            return cached.AvailabilityList;

        return new List<AvailabilityRoomDomain>();

    }

    public void Insert(params AvailabilityRoomDomain[] availabilityRooms)
    {
        foreach (var roomGroup in availabilityRooms.GroupBy(x => x.RoomNumber))
        {
            var roomNumber = roomGroup.Key;
            var roomGroupList = roomGroup.ToList();

            _cacheStore.Add(new AvailabilityRoomCache(roomGroupList),
                                      new AvailabilityRoomCacheKey(roomGroup.Key),
                                      expirationTime: null).Wait();
        }
    }
}
