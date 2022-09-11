using System.Text.Json.Serialization;

namespace Booking.Cancun.Domain.Entities;

public class AvailabilityRoomDomain
{
    public int RoomNumber { get; private set; }

    public DateTime Date { get; private set; }
    public bool Available { get; private set; }

    [JsonConstructor]
    private AvailabilityRoomDomain()
    {
    }

    public AvailabilityRoomDomain(int roomNumber, DateTime date, bool available)
    {
        RoomNumber = roomNumber;
        Date = date;
        Available = available;
    }
}
