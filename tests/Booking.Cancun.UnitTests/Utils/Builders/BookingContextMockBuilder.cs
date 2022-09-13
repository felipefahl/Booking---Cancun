using Booking.Cancun.Infraestructure.Data.MsSql;
using EntityFrameworkCore.Testing.NSubstitute;
using Microsoft.EntityFrameworkCore;

namespace Booking.Cancun.UnitTests.Utils.Builders;

public class BookingContextMockBuilder
{
    private readonly BookingContext _context;

    public BookingContextMockBuilder()
    {
        var dbContextOptions = new DbContextOptionsBuilder<BookingContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _context = Create.MockedDbContextFor<BookingContext>
            (dbContextOptions);
    }

    public BookingContext Build()
    {
        return _context;
    }
}
