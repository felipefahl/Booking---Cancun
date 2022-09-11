using Booking.Cancun.Domain.Interfaces.Repository;
using Booking.Cancun.Infraestructure.Data.MsSql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Cancun.Infraestructure.Data.MsSql;

public class BookingContext : DbContext, IUnitOfWork
{
    public virtual DbSet<BookingOrderDb> BookingOrders { get; set; }
    public virtual DbSet<BookingOrderStayDb> BookingOrderStays { get; set; }

    public BookingContext()
    {

    }

    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        var success = await SaveChangesAsync(cancellationToken) > 0;

        return success;
    }
}
