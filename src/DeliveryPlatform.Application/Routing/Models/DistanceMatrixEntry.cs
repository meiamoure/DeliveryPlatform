using System;

namespace DeliveryPlatform.Application.Routing.Models;

public sealed record DistanceMatrixEntry(
    double DistanceKm,
    int TravelTimeMin
);
