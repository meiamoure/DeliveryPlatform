using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteNumberAndCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "deliveryplatform",
                table: "Routes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                schema: "deliveryplatform",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Code",
                schema: "deliveryplatform",
                table: "Routes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Number",
                schema: "deliveryplatform",
                table: "Routes",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Routes_Code",
                schema: "deliveryplatform",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_Number",
                schema: "deliveryplatform",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "deliveryplatform",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Number",
                schema: "deliveryplatform",
                table: "Routes");
        }
    }
}
