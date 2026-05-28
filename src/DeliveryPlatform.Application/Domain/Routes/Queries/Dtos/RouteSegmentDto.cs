using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RouteSegmentDto(
    int Order,
    Guid FromNodeId,
    Guid ToNodeId,
    double DistanceKm,
    int TravelTimeMin,
    Guid? DeliveryId
);
