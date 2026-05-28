using DeliveryPlatform.Core.Domain.Routes.Data;

namespace DeliveryPlatform.Core.Domain.Routes.Models;

public class RouteSegment
{
    private RouteSegment() { }

    internal RouteSegment(Guid id, Guid routeId, int order, Guid fromNodeId, Guid toNodeId,
                          double distanceKm, int travelTimeMin, Guid? deliveryId)
    {
        Id = id;
        RouteId = routeId;
        Order = order;
        FromNodeId = fromNodeId;
        ToNodeId = toNodeId;
        DistanceKm = distanceKm;
        TravelTimeMin = travelTimeMin;
        DeliveryId = deliveryId;
    }

    public Guid Id { get; private set; }
    public Guid RouteId { get; private set; }
    public int Order { get; private set; }
    public Guid FromNodeId { get; private set; }
    public Guid ToNodeId { get; private set; }
    public double DistanceKm { get; private set; }
    public int TravelTimeMin { get; private set; }
    public Guid? DeliveryId { get; private set; }

    internal static RouteSegment Create(Route route, CreateRouteSegmentData d)
        => new(Guid.NewGuid(), route.Id, d.Order, d.FromNodeId, d.ToNodeId,
               d.DistanceKm, d.TravelTimeMin, d.DeliveryId);

    public void Update(UpdateRouteSegmentData d)
    {
        Order = d.Order;
        DistanceKm = d.DistanceKm;
        TravelTimeMin = d.TravelTimeMin;
        DeliveryId = d.DeliveryId;
    }
}
