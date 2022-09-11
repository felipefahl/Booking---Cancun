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

        var newAvailability = new List<AvailabilityRoomDomain>();

        for (DateTime date = DateTime.Today; date <= DateTime.Today.AddDays(30); date = date.AddDays(1))
        {
            bool available = true;
            newAvailability.Add(new AvailabilityRoomDomain(roomNumber, date, available));
        }
        await _cacheStore.Add(new AvailabilityRoomCache(newAvailability),
                                  new AvailabilityRoomCacheKey(roomNumber),
                                  expirationTime: null);

        return newAvailability;

    }

    public void Insert(params AvailabilityRoomDomain[] availabilityRooms)
    {
        foreach (var roomGroup in availabilityRooms.GroupBy(x => x.RoomNumber))
        {
            var roomNumber = roomGroup.Key;
            var roomGroupList = roomGroup.ToList();
            var oldAvailability = AllByRoomAsync(roomNumber).Result;
            var newAvailability = new List<AvailabilityRoomDomain>();

            for (DateTime date = DateTime.Today; date <= DateTime.Today.AddDays(30); date = date.AddDays(1))
            {
                bool available = oldAvailability
                    .Union(roomGroupList)
                    .Where(x => x.Date.Date == date)
                    .Where(x => x.RoomNumber == roomNumber)
                    .All(x => x.Available);

                newAvailability.Add(new AvailabilityRoomDomain(roomNumber, date, available));
            }

            _cacheStore.Add(new AvailabilityRoomCache(newAvailability),
                                      new AvailabilityRoomCacheKey(roomGroup.Key),
                                      expirationTime: null).Wait();
        }
    }
}
