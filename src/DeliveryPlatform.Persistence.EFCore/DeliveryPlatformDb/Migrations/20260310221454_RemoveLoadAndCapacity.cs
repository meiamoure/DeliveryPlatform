using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLoadAndCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                schema: "deliveryplatform",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Load",
                schema: "deliveryplatform",
                table: "Deliveries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                schema: "deliveryplatform",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Load",
                schema: "deliveryplatform",
                table: "Deliveries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
