using Booking.Cancun.Domain.Dtos.Request;
using Booking.Cancun.Domain.Entities;

namespace Booking.Cancun.UnitTests.Utils.Seeds;

public static class BookingOrderDomainTestsSeed
{

    public static BookingOrderDomain ValidFirst3DaysBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(3),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain ValidLast3DaysBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(28),
            EndDate = DateTime.Today.AddDays(30),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain Valid1DayBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(10),
            EndDate = DateTime.Today.AddDays(10),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidTodayBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today,
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidMoreThan3DaysBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(8),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidEmailNullBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = null,
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(5),
        };

        return new BookingOrderDomain(dto);
    }


    public static BookingOrderDomain InvalidEmailBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email",
            StartDate = DateTime.Today.AddDays(5),
            EndDate = DateTime.Today.AddDays(5),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidEmailmoreThan100CharactersBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "emailemailemailemailemailemailemailemailemailemailemailemailemailemailemailemailemailemailemailemail@email.com",
            StartDate = DateTime.Today.AddDays(6),
            EndDate = DateTime.Today.AddDays(5),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidStartDateEmptyBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            EndDate = DateTime.Today.AddDays(5),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidStartDate30DaysInAdvanceBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(31),
            EndDate = DateTime.Today.AddDays(31),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidEndDateEmptyBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(5),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidEndDate30DaysInAdvanceBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(30),
            EndDate = DateTime.Today.AddDays(31),
        };

        return new BookingOrderDomain(dto);
    }

    public static BookingOrderDomain InvalidEndDateLowerThanStartDateBookingOrderDomain()
    {
        var dto = new BookingOrderRequestDTO
        {
            Email = "email@email.com",
            StartDate = DateTime.Today.AddDays(2),
            EndDate = DateTime.Today.AddDays(1),
        };

        return new BookingOrderDomain(dto);
    }
}
