using System;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.BuildManyRoutes;

public sealed record BuildManyRoutesResult(
    IReadOnlyList<BuildManyRouteItemResult> Routes,
    IReadOnlyList<Guid> UnassignedDeliveryIds
);
