using Booking.Cancun.Domain.Dtos.Response;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Domain.Logging;

namespace Booking.Cancun.Application.BusinessCases;

public class AvailabilityService : IAvailabilityService
{
    private readonly ILoggerManager _logger;
    private readonly IAvailabilityRoomRepository _availabilityRoomRepository;
    private readonly IBookingOrderRepository _bookingOrderRepository;


    public AvailabilityService(ILoggerManager logger,
        IAvailabilityRoomRepository availabilityRoomRepository,
        IBookingOrderRepository bookingOrderRepository)
    {
        _logger = logger;
        _availabilityRoomRepository = availabilityRoomRepository;
        _bookingOrderRepository = bookingOrderRepository;
    }

    public async Task<IList<AvailabilityResponseDTO>> GetRoomAvailability(int roomNumber)
    {
        _logger.LogInfo("GetRoomAvailability");
        var result = await _availabilityRoomRepository.AllByRoomAsync(roomNumber);

        if(result.Any())
            return result.Select(x => new AvailabilityResponseDTO(x)).ToList();

        try
        {
            var stays = await _bookingOrderRepository.AllStaysDateByRoomPeriodAsync(roomNumber, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));

            var newAvailability = new List<AvailabilityRoomDomain>();

            for (DateTime date = DateTime.Today.AddDays(1); date <= DateTime.Today.AddDays(30); date = date.AddDays(1))
            {

                var staysFound = stays
                    .Where(x => x.Date.Date == date)
                    .Any();

                bool available = staysFound ? false : true;

                newAvailability.Add(new AvailabilityRoomDomain(roomNumber, date, available));

                _availabilityRoomRepository.Insert(newAvailability.ToArray());

                result = newAvailability;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e);
        }

        return result.Select(x => new AvailabilityResponseDTO(x)).ToList();
    }

    public async Task SetAvailabilityStays(IList<AvailabilityRoomDomain> availabilityRooms)
    {
        _logger.LogInfo("SetAvailabilityStays");
        var newAvailability = new List<AvailabilityRoomDomain>();

        foreach (var roomGroup in availabilityRooms.GroupBy(x => x.RoomNumber))
        {
            var roomNumber = roomGroup.Key;
            var roomGroupList = roomGroup.ToList();
            var oldAvailability = await _availabilityRoomRepository.AllByRoomAsync(roomNumber);

            for (DateTime date = DateTime.Today.AddDays(1); date <= DateTime.Today.AddDays(30); date = date.AddDays(1))
            {
                bool available = true;

                var roomGroupListFound = roomGroupList
                    .Where(x => x.Date.Date == date)
                    .FirstOrDefault();

                if (roomGroupListFound != null)
                    available = roomGroupListFound.Available;
                else
                {
                    available =
                        oldAvailability
                        .Where(x => x.Date.Date == date)
                        .Where(x => x.RoomNumber == roomNumber)
                        .All(x => x.Available);
                }

                newAvailability.Add(new AvailabilityRoomDomain(roomNumber, date, available));
            }

        }

        _availabilityRoomRepository.Insert(newAvailability.ToArray());

        await Task.CompletedTask;
    }
}
