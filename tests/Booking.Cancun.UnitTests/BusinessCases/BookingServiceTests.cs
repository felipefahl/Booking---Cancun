using Booking.Cancun.Application.BusinessCases;
using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Enums;
using Booking.Cancun.Domain.Interfaces.Jobs;
using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Domain.Logging;
using Booking.Cancun.UnitTests.Utils.Seeds;

namespace Booking.Cancun.UnitTests.BusinessCases;

public class BookingServiceTests
{
    private readonly ILoggerManager _logger;
    private readonly IBookingOrderRepository _bookingOrderRepository;
    private readonly IBookingOrderAvailabilityJob _bookedBookingOrderJob;
    private readonly IBookingNotificationJob _bookingNotificationJob;

    private BookingService _bookingService;

    public BookingServiceTests()
    {
        _logger = Substitute.For<ILoggerManager>();
        _bookingOrderRepository = Substitute.For<IBookingOrderRepository>();
        _bookedBookingOrderJob = Substitute.For<IBookingOrderAvailabilityJob>();
        _bookingNotificationJob = Substitute.For<IBookingNotificationJob>();

        _bookingService = new BookingService(_logger,
            _bookingOrderRepository,
            _bookedBookingOrderJob,
            _bookingNotificationJob
            );
    }

    [Fact]
    public async Task CancelBookingOrder_WhenExists_MustCallRepositoryDelete()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        var id = bookingDomain.Id;

        _bookingOrderRepository.FindAsync(id).Returns(bookingDomain);

        // Act
        await _bookingService.CancelBookingOrder(id);

        // Assert
        _bookingOrderRepository.Received(1).Update(bookingDomain);
        _bookingOrderRepository.Received(1).DeleteBookingStay(id);

        await _bookingOrderRepository.Received(1).CommitAsync();

        await _bookedBookingOrderJob.Send(bookingDomain);
        await _bookingNotificationJob.Send(bookingDomain);
    }

    [Fact]
    public async Task CancelBookingOrder_WhenPeriodAvailable_MustCallBookedBookingOrderJobe()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.GenerateStays();

        _bookingOrderRepository.PeriodAvailableAsync(bookingDomain.RoomNumber,
            bookingDomain.Id,
            bookingDomain.StartDate,
            bookingDomain.EndDate)
            .Returns(true);

        // Act
        await _bookingService.StayBookingOrder(bookingDomain);

        // Assert
        Assert.Equal(EBookingOrderStatus.Booked, bookingDomain.Status);
        _bookingOrderRepository.Received(1).Insert(bookingDomain);
        _bookingOrderRepository.Received(1).InsertStay(bookingDomain.Stays.ToArray());

        await _bookingOrderRepository.Received(1).CommitAsync();

        await _bookedBookingOrderJob.Received(1).Send(bookingDomain);
        await _bookingNotificationJob.Received(1).Send(bookingDomain);
    }

    [Fact]
    public async Task CancelBookingOrder_WhenPeriodNotAvailable_MustNotCallBookedBookingOrderJobe()
    {
        // Arrange
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        bookingDomain.GenerateStays();

        _bookingOrderRepository.PeriodAvailableAsync(bookingDomain.RoomNumber,
            bookingDomain.Id,
            bookingDomain.StartDate,
            bookingDomain.EndDate)
            .Returns(false);

        // Act
        await _bookingService.StayBookingOrder(bookingDomain);

        // Assert
        Assert.Equal(EBookingOrderStatus.Denied, bookingDomain.Status);
        _bookingOrderRepository.Received(1).Insert(bookingDomain);
        _bookingOrderRepository.DidNotReceive().InsertStay(bookingDomain.Stays.ToArray());

        await _bookingOrderRepository.Received(1).CommitAsync();

        await _bookedBookingOrderJob.DidNotReceive().Send(bookingDomain);
        await _bookingNotificationJob.Received(1).Send(bookingDomain);
    }

    [Fact]
    public async Task UpdateBookingOrder_WhenPeriodAvailable_MustCallBookedBookingOrderJobe()
    {
        // Arrange
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(10),
            EndDate = DateTime.Today.AddDays(10),
        };
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        var id = bookingDomain.Id;

        bookingDomain.GenerateStays();

        _bookingOrderRepository.FindAsync(id).Returns(bookingDomain);
        _bookingOrderRepository.PeriodAvailableAsync(1,
            id,
            dto.StartDate,
            dto.EndDate)
            .Returns(true);

        // Act
        await _bookingService.UpdateBookingOrder(id, dto);

        // Assert
        Assert.Equal(EBookingOrderStatus.Updated, bookingDomain.Status);

        _bookingOrderRepository.Received(1).DeleteBookingStay(bookingDomain.Id);
        _bookingOrderRepository.Received(1).Update(bookingDomain);
        _bookingOrderRepository.Received(1).InsertStay(bookingDomain.Stays.ToArray());

        await _bookingOrderRepository.Received(1).CommitAsync();

        await _bookedBookingOrderJob.Received(1).Send(bookingDomain);
        await _bookingNotificationJob.Received(1).Send(bookingDomain);
    }

    [Fact]
    public async Task UpdateBookingOrder_WhenPeriodNotAvailable_MustNotCallBookedBookingOrderJobe()
    {
        // Arrange
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(10),
            EndDate = DateTime.Today.AddDays(10),
        };
        var bookingDomain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
        var id = bookingDomain.Id;

        bookingDomain.GenerateStays();

        _bookingOrderRepository.FindAsync(id).Returns(bookingDomain);
        _bookingOrderRepository.PeriodAvailableAsync(1,
            id,
            dto.StartDate,
            dto.EndDate)
            .Returns(false);

        // Act
        await _bookingService.UpdateBookingOrder(id, dto);

        // Assert
        Assert.Equal(EBookingOrderStatus.UpdateDenied, bookingDomain.Status);
        _bookingOrderRepository.Received(1).Update(bookingDomain);
        _bookingOrderRepository.DidNotReceive().InsertStay(bookingDomain.Stays.ToArray());

        await _bookingOrderRepository.Received(1).CommitAsync();

        await _bookedBookingOrderJob.DidNotReceive().Send(bookingDomain);
        await _bookingNotificationJob.Received(1).Send(bookingDomain);
    }
}
