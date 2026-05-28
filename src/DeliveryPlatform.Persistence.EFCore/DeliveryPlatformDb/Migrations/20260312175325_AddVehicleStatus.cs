using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentRouteId",
                schema: "deliveryplatform",
                table: "Vehicles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "deliveryplatform",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRouteId",
                schema: "deliveryplatform",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "deliveryplatform",
                table: "Vehicles");
        }
    }
}
