using AutoMapper;
using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Enums;
using Booking.Cancun.Infraestructure.Data.MsSql;
using Booking.Cancun.Infraestructure.Data.MsSql.Entities;
using Booking.Cancun.Infraestructure.Data.MsSql.Mappings;
using Booking.Cancun.Infraestructure.Data.MsSql.Repository;
using Booking.Cancun.UnitTests.Utils.Builders;
using Booking.Cancun.UnitTests.Utils.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Booking.Cancun.UnitTests.Repositories;

public class BookingOrderRepositoryTests
{
    private readonly BookingContext _context;
    private readonly IMapper _mapper;

    private BookingOrderRepository _repository;

    private BookingOrderDomain booking1Domain = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();
    private BookingOrderDomain booking2Domain = BookingOrderDomainTestsSeed.ValidFirst3DaysBookingOrderDomain();
    private BookingOrderDomain booking3Domain = BookingOrderDomainTestsSeed.ValidLast3DaysBookingOrderDomain();

    public BookingOrderRepositoryTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new GeneralProfileData());
        });
        _mapper = mappingConfig.CreateMapper();

        _context = new BookingContextMockBuilder().Build();

        booking1Domain.GenerateStays();
        booking2Domain.GenerateStays();
        booking3Domain.GenerateStays();

        _context.BookingOrders.Add(new BookingOrderDb(booking1Domain));
        _context.BookingOrders.Add(new BookingOrderDb(booking2Domain));
        _context.BookingOrders.Add(new BookingOrderDb(booking3Domain));

        var orderStaysDb = booking1Domain.Stays
            .Union(booking2Domain.Stays)
            .Union(booking3Domain.Stays)
            .Select(x => new BookingOrderStayDb(x));

        _context.BookingOrderStays.AddRange(orderStaysDb);

        _context.SaveChanges();

        foreach (var entry in _context.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        _repository = new BookingOrderRepository(_context, _mapper);
    }

    [Fact]
    public async Task AllStaysDateByRoomPeriodAsync_WhenExists_MustReturnsListOfDays()
    {
        // Arrange
        var days = _context.BookingOrderStays.Select(x => x.Day).ToList();

        // Act
        var daysFound =
            await _repository.AllStaysDateByRoomPeriodAsync(1, DateTime.Today, DateTime.Today.AddDays(30));

        // Assert
        Assert.Equal(days, daysFound);
    }

    [Fact]
    public async Task AllStaysDateByRoomPeriodAsync_WhenNotExists_MustReturnsEmpty()
    {
        // Arrange

        // Act
        var daysFound =
            await _repository.AllStaysDateByRoomPeriodAsync(4, DateTime.Today, DateTime.Today.AddDays(30));

        // Assert
        Assert.Empty(daysFound);
    }

    [Fact]
    public async Task CommitAsync_WhenCalled_MustReturnsBool()
    {
        // Arrange

        // Act
        var commit =
            await _repository.CommitAsync();

        // Assert
        Assert.IsType<bool>(commit);
    }

    [Fact]
    public async Task FindAsync_WhenExists_MustReturnsEntityDomain()
    {
        // Arrange
        var idToBeFound = booking1Domain.Id;

        // Act
        var bookingFound =
            await _repository.FindAsync(idToBeFound);

        // Assert
        Assert.Equal(booking1Domain.Id, bookingFound.Id);
    }

    [Fact]
    public async Task FindAsync_WhenNotExists_MustReturnsNull()
    {
        // Arrange
        var idToBeFound = Guid.NewGuid();

        // Act
        var bookingFound =
            await _repository.FindAsync(idToBeFound);

        // Assert
        Assert.Null(bookingFound);
    }

    [Fact]
    public async Task Insert_WhenSuccess_MustExistsInRepositoy()
    {
        // Arrange
        var newBooking = new BookingOrderDomain(new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(5),
        });

        // Act
        _repository.Insert(newBooking);
        await _repository.CommitAsync();

        // Assert
        var foundBooking = await _repository.FindAsync(newBooking.Id);

        Assert.Equal(newBooking.Id, foundBooking?.Id);
    }

    [Fact]
    public async Task InsertStay_WhenSuccess_MustExistsInRepositoy()
    {
        // Arrange
        var newBooking = new BookingOrderDomain(new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(5),
        });
        newBooking.GenerateStays();
        var stays = newBooking.Stays.ToArray();
        _repository.Insert(newBooking);

        // Act
        _repository.InsertStay(stays);
        await _repository.CommitAsync();

        // Assert
        var foundBooking = await _repository.FindAsync(newBooking.Id);

        Assert.Equal(newBooking.Stays.Count(), foundBooking?.Stays?.Count());
    }

    [Fact]
    public async Task DeleteBookingStay_WhenSuccess_MustNotExistsInRepositoy()
    {
        // Arrange
        var newBooking = new BookingOrderDomain(new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(5),
        });
        newBooking.GenerateStays();
        var stays = newBooking.Stays.ToArray();
        _repository.Insert(newBooking);
        _repository.InsertStay(stays);
        await _repository.CommitAsync();

        // Act
        _repository.DeleteBookingStay(newBooking.Id);
        await _repository.CommitAsync();

        // Assert
        var foundBooking = await _repository.FindAsync(newBooking.Id);

        Assert.Empty(foundBooking?.Stays);
    }

    [Fact]
    public async Task Update_WhenSuccess_MustUpdatedEntityFields()
    {
        // Arrange
        booking1Domain.Updated();

        // Act
        _repository.Update(booking1Domain);
        await _repository.CommitAsync();

        // Assert
        var bookingFound =
            await _repository.FindAsync(booking1Domain.Id);

        Assert.Equal(booking1Domain.Id, bookingFound.Id);
        Assert.Equal(EBookingOrderStatus.Updated, bookingFound.Status);
    }

    [Fact]
    public async Task PeriodAvailableAsync_WhenExistsStays_MustReturnsFalse()
    {
        // Arrange
        var room = 1;

        // Act
        var periodAvailable =
            await _repository.PeriodAvailableAsync(room, Guid.NewGuid(), DateTime.Today, DateTime.Today.AddDays(30));

        // Assert
        Assert.False(periodAvailable);
    }

    [Fact]
    public async Task PeriodAvailableAsync_WhenExistsStays_MustReturnsTrue()
    {
        // Arrange
        var room = 2;

        // Act
        var periodAvailable =
            await _repository.PeriodAvailableAsync(room, Guid.NewGuid(), DateTime.Today, DateTime.Today.AddDays(30));

        // Assert
        Assert.True(periodAvailable);
    }
}
