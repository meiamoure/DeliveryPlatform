namespace DeliveryPlatform.Core.Domain.Routes.Data;

public record UpdateRouteSegmentData(
    int Order, double DistanceKm, int TravelTimeMin, Guid? DeliveryId);

