using DeliveryPlatform.Core.Domain.Routes.Models;
using DeliveryPlatform.Core.Domain.Routes.Data;
using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Deliveries.Models;
using DeliveryPlatform.Core.Domain.Vehicles.Models;


namespace DeliveryPlatform.Application.Routing;


public sealed class RouteBuilder
{
    public Route Create(
        IReadOnlyList<Guid> orderedNodes,
        DistanceMatrix matrix,
        Vehicle vehicle,
        DateOnly date,
        IReadOnlyDictionary<Guid, List<Delivery>> deliveriesByNode,
        Guid driverId)
    {
        if (orderedNodes.Count < 2)
            throw new InvalidOperationException("Route requires at least 2 nodes");

        var route = Route.Create(
            new CreateRouteData(vehicle.Id, date, driverId)
        );

        decimal totalWeight = 0;
        decimal totalVolume = 0;

        int segmentOrder = 0;

        for (int i = 0; i < orderedNodes.Count - 1; i++)
        {
            var from = orderedNodes[i];
            var to = orderedNodes[i + 1];

            if (from == to)
                throw new InvalidOperationException($"Route contains duplicate consecutive node: {from}");

            var entry = matrix.Get(from, to);

            if (entry.DistanceKm < 0 || entry.TravelTimeMin < 0)
                throw new InvalidOperationException(
                    $"Suspicious matrix entry {from}->{to}: {entry.DistanceKm} km, {entry.TravelTimeMin} min");

            Guid? deliveryId = null;

            if (deliveriesByNode.TryGetValue(to, out var nodeDeliveries) && nodeDeliveries.Count > 0)
            {
                deliveryId = nodeDeliveries[0].Id;
            }

            route.AddSegment(new CreateRouteSegmentData(
                Order: segmentOrder++,
                FromNodeId: from,
                ToNodeId: to,
                DistanceKm: entry.DistanceKm,
                TravelTimeMin: entry.TravelTimeMin,
                DeliveryId: deliveryId
            ));

            if (deliveriesByNode.TryGetValue(to, out nodeDeliveries))
            {
                foreach (var delivery in nodeDeliveries)
                {
                    totalWeight += delivery.WeightKg;
                    totalVolume += delivery.VolumeM3;

                    if (totalWeight > vehicle.MaxWeightKg)
                        throw new InvalidOperationException(
                            $"Vehicle capacity exceeded: weight {totalWeight} > {vehicle.MaxWeightKg}");

                    if (totalVolume > vehicle.MaxVolumeM3)
                        throw new InvalidOperationException(
                            $"Vehicle capacity exceeded: volume {totalVolume} > {vehicle.MaxVolumeM3}");
                }
            }
        }

        return route;
    }
}
