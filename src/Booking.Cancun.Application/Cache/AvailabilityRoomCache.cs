using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Application.Cache;

public class AvailabilityRoomCache
{
    public IList<AvailabilityRoomDomain> AvailabilityList { get; }

    public AvailabilityRoomCache(IList<AvailabilityRoomDomain> availabilityList)
    {
        AvailabilityList = availabilityList;
    }
}
