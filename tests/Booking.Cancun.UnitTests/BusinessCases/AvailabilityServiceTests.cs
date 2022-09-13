using Booking.Cancun.Application.BusinessCases;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Domain.Logging;

namespace Booking.Cancun.UnitTests.BusinessCases;

public  class AvailabilityServiceTests
{
    private readonly ILoggerManager _logger;
    private readonly IAvailabilityRoomRepository _availabilityRoomRepository;
    private readonly IBookingOrderRepository _bookingOrderRepository;

    private AvailabilityService _availabilityService;

    public AvailabilityServiceTests()
    {
        _logger = Substitute.For<ILoggerManager>();
        _availabilityRoomRepository = Substitute.For<IAvailabilityRoomRepository>();
        _bookingOrderRepository = Substitute.For<IBookingOrderRepository>();

        _availabilityService = new AvailabilityService(_logger,
            _availabilityRoomRepository,
            _bookingOrderRepository);            
    }


    [Fact]
    public async Task GetRoomAvailability_WhenExists_MustReturnsListOfAvailabilities()
    {
        // Arrange
        var room = 1;
        var availabilityDomainList = new List<AvailabilityRoomDomain>()
        {
            new AvailabilityRoomDomain(room, DateTime.Today.AddDays(1), false),
            new AvailabilityRoomDomain(room, DateTime.Today.AddDays(2), true),
        };

        _availabilityRoomRepository.AllByRoomAsync(room).Returns(availabilityDomainList);

        // Act
        var availabilities =
            await _availabilityService.GetRoomAvailability(1);

        // Assert
        Assert.NotEmpty(availabilities);
        await _bookingOrderRepository.DidNotReceive().AllStaysDateByRoomPeriodAsync(room, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));
        _availabilityRoomRepository.DidNotReceive().Insert(Arg.Any<AvailabilityRoomDomain>());
    }

    [Fact]
    public async Task GetRoomAvailability_WhenNotExists_MustReturnsNewListOfAvailabilities()
    {
        // Arrange
        var room = 1;

        // Act
        var availabilities =
            await _availabilityService.GetRoomAvailability(1);

        // Assert
        Assert.NotEmpty(availabilities);
        await _bookingOrderRepository.Received(1).AllStaysDateByRoomPeriodAsync(room, DateTime.Today.AddDays(1), DateTime.Today.AddDays(30));
        _availabilityRoomRepository.Received().Insert(Arg.Any<AvailabilityRoomDomain>());
    }

    [Fact]
    public async Task SetAvailabilityStays_WhenCalled_MustInsertListOfAvailabilities()
    {
        // Arrange
        var room = 1;
        var availabilityDomainList = new List<AvailabilityRoomDomain>()
        {
            new AvailabilityRoomDomain(room, DateTime.Today.AddDays(1), false),
            new AvailabilityRoomDomain(room, DateTime.Today.AddDays(2), true),
        };

        // Act
        await _availabilityService.SetAvailabilityStays(availabilityDomainList);

        // Assert
        _availabilityRoomRepository.DidNotReceive().Insert(availabilityDomainList.ToArray());
    }
}
