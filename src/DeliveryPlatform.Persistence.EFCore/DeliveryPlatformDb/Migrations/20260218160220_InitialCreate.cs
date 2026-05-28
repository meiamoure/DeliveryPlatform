using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "deliveryplatform");

            migrationBuilder.CreateTable(
                name: "Drivers",
                schema: "deliveryplatform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                schema: "deliveryplatform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Lat = table.Column<double>(type: "double precision", nullable: false),
                    Lng = table.Column<double>(type: "double precision", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                schema: "deliveryplatform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PickupNodeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Load = table.Column<int>(type: "integer", nullable: false),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    WindowStart = table.Column<TimeOnly>(type: "time", nullable: true),
                    WindowEnd = table.Column<TimeOnly>(type: "time", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliveries_Nodes_PickupNodeId",
                        column: x => x.PickupNodeId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "deliveryplatform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    SpeedKmh = table.Column<int>(type: "integer", nullable: false),
                    DepotNodeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Nodes_DepotNodeId",
                        column: x => x.DepotNodeId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                schema: "deliveryplatform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RouteSegments",
                schema: "deliveryplatform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    FromNodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToNodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DistanceKm = table.Column<double>(type: "double precision", nullable: false),
                    TravelTimeMin = table.Column<int>(type: "integer", nullable: false),
                    DeliveryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteSegments_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteSegments_Routes_RouteId",
                        column: x => x.RouteId,
                        principalSchema: "deliveryplatform",
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_NodeId",
                schema: "deliveryplatform",
                table: "Deliveries",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_PickupNodeId",
                schema: "deliveryplatform",
                table: "Deliveries",
                column: "PickupNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_Status_ServiceDate",
                schema: "deliveryplatform",
                table: "Deliveries",
                columns: new[] { "Status", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_Name",
                schema: "deliveryplatform",
                table: "Nodes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DriverId",
                schema: "deliveryplatform",
                table: "Routes",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_VehicleId",
                schema: "deliveryplatform",
                table: "Routes",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteSegments_DeliveryId",
                schema: "deliveryplatform",
                table: "RouteSegments",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteSegments_RouteId_Order",
                schema: "deliveryplatform",
                table: "RouteSegments",
                columns: new[] { "RouteId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DepotNodeId",
                schema: "deliveryplatform",
                table: "Vehicles",
                column: "DepotNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                schema: "deliveryplatform",
                table: "Vehicles",
                column: "Plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteSegments",
                schema: "deliveryplatform");

            migrationBuilder.DropTable(
                name: "Deliveries",
                schema: "deliveryplatform");

            migrationBuilder.DropTable(
                name: "Routes",
                schema: "deliveryplatform");

            migrationBuilder.DropTable(
                name: "Drivers",
                schema: "deliveryplatform");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "deliveryplatform");

            migrationBuilder.DropTable(
                name: "Nodes",
                schema: "deliveryplatform");
        }
    }
}
