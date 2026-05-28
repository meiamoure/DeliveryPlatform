using System;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

public sealed record RoutePathPointDto(
    double Lat,
    double Lng
);
