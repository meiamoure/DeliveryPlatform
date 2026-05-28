using System;

namespace DeliveryPlatform.Api.Controllers.Routes.Requests;

public sealed record BuildManyRoutesRequest(
    Guid DepotNodeId,
    List<Guid> VehicleIds,
    List<Guid> DeliveryIds,
    DateTime Date
);
