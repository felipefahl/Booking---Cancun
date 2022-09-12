using Booking.Cancun.Domain.Enums;
using Booking.Cancun.Infraestructure.Data.MsSql.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Cancun.Infraestructure.Data.MsSql.Configurations;

public class BookingOrderDbConfiguration : IEntityTypeConfiguration<BookingOrderDb>
{
    public void Configure(EntityTypeBuilder<BookingOrderDb> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(c => c.BookedAt)
            .IsRequired();

        builder.Property(c => c.StartDate)
            .IsRequired();

        builder.Property(c => c.EndDate)
            .IsRequired();

        builder.Property(c => c.RoomNumber)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder
        .Property(e => e.Status)
        .HasConversion(
            v => v.ToString(),
            v => (EBookingOrderStatus)Enum.Parse(typeof(EBookingOrderStatus), v));

        builder.ToTable("BookingOrders");
    }
}
