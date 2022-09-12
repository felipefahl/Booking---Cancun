using Booking.Cancun.Application.Producers;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.Infraestructure.CrossCutting.Consumers;
using DotNetCore.CAP;

namespace Booking.Cancun.Application.Consumers;

public class BookingOrderAvailabilityConsumer : KafkaConsumer<BookingOrderDomain>
{
    private readonly ILoggerManager _logger;
    private readonly IAvailabilityService _availabilityService;
    public BookingOrderAvailabilityConsumer(ILoggerManager logger, IAvailabilityService availabilityService)
        : base()
    {
        _logger = logger;
        _availabilityService = availabilityService;
    }

    [CapSubscribe(nameof(BookingOrderAvailabilityProducer))]
    public override async Task ConsumeMessageAsync(string stringMessage, CancellationToken cancellationToken)
    {
        _logger.LogInfo("BookingOrderJobAvailabilityJob");

        var message = ConvertMessage(stringMessage);

        IList<AvailabilityRoomDomain> availabilityRooms = new List<AvailabilityRoomDomain>();

        var stayDays = message.Stays.Select(x => x.Day).ToList();
        var oldStays = message.OldStays.Select(x => x.Day).ToList();

        foreach (var stayDay in stayDays)
        {
            var availabilityRoom = new AvailabilityRoomDomain(message.RoomNumber, stayDay, false);
            availabilityRooms.Add(availabilityRoom);
        }
        foreach (var oldStay in oldStays.Where(x => !stayDays.Contains(x)))
        {
            var availabilityRoom = new AvailabilityRoomDomain(message.RoomNumber, oldStay, true);
            availabilityRooms.Add(availabilityRoom);
        }

        await _availabilityService.SetAvailabilityStays(availabilityRooms);
    }
}
