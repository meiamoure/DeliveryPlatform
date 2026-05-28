using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Data;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Domain.Nodes.Models;

namespace DeliveryPlatform.Core.Domain.Vehicles.Models;

public class Vehicle : IAggregateRoot
{
    private Vehicle() { }

    internal Vehicle(
        Guid id,
        string plate,
        decimal maxWeightKg,
        decimal maxVolumeM3,
        int speedKmh,
        Guid depotNodeId)
    {
        Id = id;
        Plate = plate;
        MaxWeightKg = maxWeightKg;
        MaxVolumeM3 = maxVolumeM3;
        SpeedKmh = speedKmh;
        DepotNodeId = depotNodeId;

        Status = VehicleStatus.Available;
    }

    public Guid Id { get; private set; }

    public string Plate { get; private set; } = default!;

    public decimal MaxWeightKg { get; private set; }

    public decimal MaxVolumeM3 { get; private set; }

    public int SpeedKmh { get; private set; }

    public Guid DepotNodeId { get; private set; }

    public Node DepotNode { get; private set; } = null!;

    public VehicleStatus Status { get; private set; }

    public Guid? CurrentRouteId { get; private set; }

    public Guid DriverId { get; private set; }

    public static Vehicle Create(CreateVehicleData d)
        => new(
            Guid.NewGuid(),
            d.Plate,
            d.MaxWeightKg,
            d.MaxVolumeM3,
            d.SpeedKmh,
            d.DepotNodeId
        );

    public void Update(UpdateVehicleData d)
    {
        Plate = d.Plate;
        MaxWeightKg = d.MaxWeightKg;
        MaxVolumeM3 = d.MaxVolumeM3;
        SpeedKmh = d.SpeedKmh;
        DepotNodeId = d.DepotNodeId;
    }

    public void AssignToRoute(Guid routeId)
    {
        if (Status == VehicleStatus.Busy)
            throw new InvalidOperationException("Vehicle already assigned");

        CurrentRouteId = routeId;
        Status = VehicleStatus.Busy;
    }

    public void Unassign()
    {
        CurrentRouteId = null;
        Status = VehicleStatus.Available;
    }

    public void SetStatus(VehicleStatus status)
    {
        if (CurrentRouteId.HasValue && status == VehicleStatus.Busy)
        {
            throw new InvalidOperationException("Cannot deactivate assigned vehicle.");
        }

        Status = status;
    }

    public void AssignDriver(Guid driverId)
    {
        DriverId = driverId;
    }

}
