using Booking.Cancun.Infraestructure.Data.MsSql.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Cancun.Infraestructure.Data.MsSql.Configurations;

public class BookingOrderStayDbConfiguration : IEntityTypeConfiguration<BookingOrderStayDb>
{
    public void Configure(EntityTypeBuilder<BookingOrderStayDb> builder)
    {
        builder.HasKey(x => new { x.BookingOrderId, x.Day });

        builder.HasOne(c => c.BookingOrder)
            .WithMany(c => c.Stays)
            .HasForeignKey(p => p.BookingOrderId);

        builder.ToTable("BookingOrderStays");
    }
}
