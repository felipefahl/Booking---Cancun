using Booking.Cancun.CrossCutting.Logging;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.BusinessCases;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Infraestructure.CrossCutting.Jobs;

namespace Booking.Cancun.Application.Producers;

public class BookedBookingOrderJob : HangFireJob<BookingOrderDomain>, IBookedBookingOrderJob
{
    private readonly IAvailabilityService _availabilityService;
    public BookedBookingOrderJob(ILoggerManager logger, IAvailabilityService availabilityService)
        : base(logger)
    {
        _availabilityService = availabilityService;
    }

    public async Task Send(BookingOrderDomain bookingOrderDomain)
    {
        await Proccess(bookingOrderDomain);
    }

    public async override Task ProccessMessage(BookingOrderDomain message)
    {
        IList<AvailabilityRoomDomain> availabilityRooms = message
            .Stays
            .Select(x => new AvailabilityRoomDomain(message.RoomNumber,
                x.Day,
                false))
            .ToList();

        await _availabilityService.SetAvailabilityStays(availabilityRooms);
    }
}
