using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightVolumeAndProductGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxWeightKg",
                table: "Vehicles",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxVolumeM3",
                table: "Vehicles",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightKg",
                table: "Deliveries",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VolumeM3",
                table: "Deliveries",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductGroup",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);

            // Заполняем новые поля в Vehicles
            migrationBuilder.Sql(@"
            UPDATE Vehicles
            SET MaxWeightKg = CAST(Capacity AS decimal(10,2))
            WHERE MaxWeightKg = 0
        ");

            migrationBuilder.Sql(@"
            UPDATE Vehicles
            SET MaxVolumeM3 = 10.00
            WHERE MaxVolumeM3 = 0
        ");

            // Заполняем новые поля в Deliveries
            migrationBuilder.Sql(@"
            UPDATE Deliveries
            SET WeightKg = CAST([Load] AS decimal(10,2))
            WHERE WeightKg = 0
        ");

            migrationBuilder.Sql(@"
            UPDATE Deliveries
            SET VolumeM3 = 1.00
            WHERE VolumeM3 = 0
        ");

            migrationBuilder.Sql(@"
            UPDATE Deliveries
            SET ProductGroup = 4
            WHERE ProductGroup = 0
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxWeightKg",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MaxVolumeM3",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "VolumeM3",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ProductGroup",
                table: "Deliveries");
        }
    }
}
