using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RouteStopDto(
    Guid NodeId,
    string NodeName,
    string? OrderNumber,
    double Lat,
    double Lng,
    int Sequence
);
