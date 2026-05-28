using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RouteDetailsDto(
    Guid Id,
    Guid VehicleId,
    string VehiclePlate,
    Guid? DriverId,
    string? DriverName,
    DateOnly ServiceDate,
    string Status,
    IReadOnlyList<RoutePointDto> OrderedPoints,
    IReadOnlyList<RouteSegmentDetailsDto> Segments,
    IReadOnlyList<RoutePathPointDto> Path,
    IReadOnlyList<RouteStopDto> Stops,
    double TotalDistanceKm,
    int TotalTimeMin,
    int Number,
    string Code
);
