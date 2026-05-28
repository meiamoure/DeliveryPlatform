using System;

namespace DeliveryPlatform.Infrastructure.Routing.Graph;

public sealed class RoadEdge
{
    public Guid FromNodeId { get; init; }
    public Guid ToNodeId { get; init; }

    public double DistanceKm { get; init; }
    public int TravelTimeMin { get; init; }
}
