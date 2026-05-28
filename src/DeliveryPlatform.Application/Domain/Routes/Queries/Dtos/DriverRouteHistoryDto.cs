namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record DriverRouteHistoryDto(
    Guid Id,
    DateOnly ServiceDate,
    string Status,
    double TotalDistanceKm,
    int TotalTimeMin,
    int Number,
    string Code
);
