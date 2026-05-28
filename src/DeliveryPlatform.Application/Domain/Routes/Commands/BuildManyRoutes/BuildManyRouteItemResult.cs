using System;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.BuildManyRoutes;

public sealed record BuildManyRouteItemResult(
    Guid RouteId,
    Guid VehicleId,
    string VehiclePlate,
    int DeliveriesCount
);
