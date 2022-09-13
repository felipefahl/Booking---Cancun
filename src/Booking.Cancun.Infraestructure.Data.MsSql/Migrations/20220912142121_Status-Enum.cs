using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Booking.Cancun.Infraestructure.Data.MsSql.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class StatusEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BookingOrders");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BookingOrders",
                newName: "BookedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookedAt",
                table: "BookingOrders",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BookingOrders",
                type: "datetime2",
                nullable: true);
        }
    }
}
