namespace DeliveryPlatform.Core.Domain.Routes.Data;

public record CreateRouteSegmentData(
    int Order, Guid FromNodeId, Guid ToNodeId,
    double DistanceKm, int TravelTimeMin, Guid? DeliveryId = null);
