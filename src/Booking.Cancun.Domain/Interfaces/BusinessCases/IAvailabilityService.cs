using Booking.Cancun.Domain.Dtos.Response;
using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Interfaces.BusinessCases;

public interface IAvailabilityService
{
    Task SetAvailabilityStays(IList<AvailabilityRoomDomain> availabilityRooms);
    Task<IList<AvailabilityResponseDTO>> GetRoomAvailability(int roomNumber);
}
