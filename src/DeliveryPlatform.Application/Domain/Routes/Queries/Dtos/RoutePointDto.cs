using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RoutePointDto(
    Guid NodeId,
    double Lat,
    double Lng
);
