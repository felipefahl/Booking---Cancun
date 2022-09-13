using Booking.Cancun.Domain.Entities;
using Booking.Cancun.Domain.Exceptions;
using Booking.Cancun.UnitTests.Utils.Seeds;

namespace Booking.Cancun.UnitTests.Entities.Domain;

public class BookingOrderDomainTests
{
    [Fact]
    public void CreateBookingOrder_WhenValid_MustNotBeNull()
    {
        // Arrange

        // Act
        var order = BookingOrderDomainTestsSeed.ValidLast3DaysBookingOrderDomain();

        // Assert
        Assert.NotNull(order);
        Assert.IsType<BookingOrderDomain>(order);
    }

    [Fact]
    public void CreateBookingOrder_WhenValid1DayBooking_MustNotBeNull()
    {
        // Arrange

        // Act
        var order = BookingOrderDomainTestsSeed.Valid1DayBookingOrderDomain();

        // Assert
        Assert.NotNull(order);
        Assert.IsType<BookingOrderDomain>(order);
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidTodayBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidTodayBookingOrderDomain());

        // Assert
        Assert.Contains("Should be in future", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidMoreThan3DaysBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidMoreThan3DaysBookingOrderDomain());

        // Assert
        Assert.Contains("Should not stay more than 3 days", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidEmailNullBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidEmailNullBookingOrderDomain());

        // Assert
        Assert.Contains("Is Required", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidEmailBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidEmailBookingOrderDomain());

        // Assert
        Assert.Contains("Should be a valid Email", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidEmailmoreThan100CharactersBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidEmailmoreThan100CharactersBookingOrderDomain());

        // Assert
        Assert.Contains("Size should be a less than 100 carach characters", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidStartDateEmptyBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidStartDateEmptyBookingOrderDomain());

        // Assert
        Assert.Contains("Is Required", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidInvalidStartDate30DaysInAdvanceBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidStartDate30DaysInAdvanceBookingOrderDomain());

        // Assert
        Assert.Contains("Should be in the next 30 days", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidEndDateEmptyBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidEndDateEmptyBookingOrderDomain());

        // Assert
        Assert.Contains("Is Required", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidInvalidEndDate30DaysInAdvanceBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidEndDate30DaysInAdvanceBookingOrderDomain());

        // Assert
        Assert.Contains("Should be in the next 30 days", ex.DomainNotifications.Select(x => x.Value));
    }

    [Fact]
    public void CreateBookingOrder_WhenInvalidEndDateLowerThanStartDateBooking_MustThrowDomainException()
    {
        // Arrange

        // Act
        var ex = Assert.Throws<DomainException>(() => BookingOrderDomainTestsSeed.InvalidEndDateLowerThanStartDateBookingOrderDomain());

        // Assert
        Assert.Contains("Should be greater or equal than StartDate", ex.DomainNotifications.Select(x => x.Value));
    }
}
