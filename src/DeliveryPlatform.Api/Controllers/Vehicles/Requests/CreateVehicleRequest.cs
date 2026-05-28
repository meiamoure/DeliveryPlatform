using System;

namespace DeliveryPlatform.Api.Controllers.Vehicles.Requests;

public sealed record CreateVehicleRequest(
    string Plate,
    decimal MaxWeightKg,
    decimal MaxVolumeM3,
    int SpeedKmh,
    Guid DepotNodeId
);
