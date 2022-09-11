using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.Repository;

public interface IAvailabilityRoomRepository
{
    void Insert(params AvailabilityRoomDomain[] availabilityRooms);
    Task<IEnumerable<AvailabilityRoomDomain>> AllByRoomAsync(int roomNumber);
}
