using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Dtos.Response;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Repository;

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
        var result = await _availabilityRoomRepository.AllByRoomAsync(roomNumber);

        try
        {
            var stays = await _bookingOrderRepository.AllStaysDateByRoomPeriodAsync(roomNumber, DateTime.Today, DateTime.Today.AddDays(30));

            if (stays.Any())
            {
                var availabilityToUpdate = stays.Select(x => new AvailabilityRoomDomain(roomNumber, x, false)).ToList();

                await SetAvailabilityStays(availabilityToUpdate);

                result = await _availabilityRoomRepository.AllByRoomAsync(roomNumber);
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
        _availabilityRoomRepository.Insert(availabilityRooms.ToArray());

        await Task.CompletedTask;
    }
}
