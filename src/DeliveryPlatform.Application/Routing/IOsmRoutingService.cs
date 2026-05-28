using System;
using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;

namespace DeliveryPlatform.Application.Routing;

public interface IOsmRoutingService
{
    Task<(double DistanceKm, double DurationMin)> GetRouteAsync(
        double fromLat, double fromLng,
        double toLat, double toLng,
        CancellationToken ct);

    Task<OsrmTableResult> GetTableAsync(
        List<(double lat, double lng)> coords,
        CancellationToken ct);

    Task<IReadOnlyList<(double lat, double lng)>> GetRouteGeometryAsync(
        List<(double lat, double lng)> coords,
        CancellationToken ct);
}
