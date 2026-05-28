using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RouteListItemDto(
    Guid Id,
    Guid VehicleId,
    string VehiclePlate,
    Guid? DriverId,
    DateOnly ServiceDate,
    string Status,
    int SegmentsCount,
    double TotalDistanceKm,
    int TotalTimeMin,
    int Number,
    string Code
);
