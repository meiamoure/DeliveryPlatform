using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record DriverRouteDto(
    Guid Id,
    string VehiclePlate,
    DateOnly ServiceDate,
    string Status,

    IReadOnlyList<RoutePointDto> OrderedPoints,
    IReadOnlyList<RoutePathPointDto> Path,
    IReadOnlyList<RouteStopDto> Stops,

    double TotalDistanceKm,
    int TotalTimeMin,

    int Number,
    string Code
);
