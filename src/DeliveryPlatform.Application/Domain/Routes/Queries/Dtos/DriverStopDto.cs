using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record DriverStopDto(
    int Order,
    Guid NodeId,
    string Title,
    double Lat,
    double Lng,
    string? OrderNumber
);
