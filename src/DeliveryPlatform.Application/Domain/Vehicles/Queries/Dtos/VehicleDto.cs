using DeliveryPlatform.Core.Domain.Vehicles.Common;

namespace DeliveryPlatform.Application.Domain.Vehicles.Queries.Dtos;

public sealed record VehicleDto(
    Guid Id,
    string Plate,
    decimal MaxWeightKg,
    decimal MaxVolumeM3,
    int SpeedKmh,
    string Status,
    Guid? DepotNodeId,
    string? DepotName,
    Guid? CurrentRouteId
);
