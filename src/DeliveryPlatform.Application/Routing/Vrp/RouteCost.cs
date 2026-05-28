using System;
using DeliveryPlatform.Application.Routing.Models;

namespace DeliveryPlatform.Application.Routing.Vrp;

public static class RouteCost
{
    public static int TotalTimeMin(IReadOnlyList<Guid> route, DistanceMatrix matrix)
    {
        var total = 0;
        for (int i = 0; i < route.Count - 1; i++)
            total += matrix.Get(route[i], route[i + 1]).TravelTimeMin;
        return total;
    }

    public static double TotalDistanceKm(IReadOnlyList<Guid> route, DistanceMatrix matrix)
    {
        double total = 0;
        for (int i = 0; i < route.Count - 1; i++)
            total += matrix.Get(route[i], route[i + 1]).DistanceKm;
        return total;
    }
}
