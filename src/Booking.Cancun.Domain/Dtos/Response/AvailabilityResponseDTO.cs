using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.Domain.Dtos.Response;

public class AvailabilityResponseDTO
{
    public int RoomNumber { get; private set; }

    public DateTime Date { get; private set; }
    public bool Available { get; private set; }

    public AvailabilityResponseDTO(AvailabilityRoomDomain availabilityRoomDomain)
    {
        RoomNumber = availabilityRoomDomain.RoomNumber;
        Date = availabilityRoomDomain.Date;
        Available = availabilityRoomDomain.Available;
    }
}
