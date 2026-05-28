using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RouteSegmentDetailsDto(
    int Order,
    Guid FromNodeId,
    string FromNodeName,
    Guid ToNodeId,
    string ToNodeName,
    double DistanceKm,
    int TravelTimeMin,
    Guid? DeliveryId
);
