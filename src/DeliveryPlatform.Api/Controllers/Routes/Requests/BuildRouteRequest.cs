using System;

namespace DeliveryPlatform.Api.Controllers.Routes.Requests;

public sealed record BuildRouteRequest(
    Guid DepotNodeId,
    Guid VehicleId,
    List<Guid> DeliveryIds,
    DateTime Date
);
