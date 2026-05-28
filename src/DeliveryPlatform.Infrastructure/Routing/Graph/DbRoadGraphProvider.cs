using System;
using DeliveryPlatform.Infrastructure.Routing.Geo;
using DeliveryPlatform.Core.Domain.Nodes.Common;

namespace DeliveryPlatform.Infrastructure.Routing.Graph;

public sealed class DbRoadGraphProvider : IRoadGraphProvider
{
    private readonly INodeRepository _nodes;

    public DbRoadGraphProvider(INodeRepository nodes)
    {
        _nodes = nodes;
    }

    public async Task<RoadGraph> GetGraphAsync(CancellationToken ct)
    {
        var graph = new RoadGraph();

        var nodes = await _nodes.GetAllAsync(ct);

        foreach (var from in nodes)
        {
            foreach (var to in nodes)
            {
                if (from.Id == to.Id)
                    continue;

                var distanceKm = GeoDistanceCalculator.HaversineKm(
                    from.Lat, from.Lng,
                    to.Lat, to.Lng);

                var travelTimeMin = (int)Math.Round(distanceKm / 40 * 60);

                graph.AddEdge(new RoadEdge
                {
                    FromNodeId = from.Id,
                    ToNodeId = to.Id,
                    DistanceKm = distanceKm,
                    TravelTimeMin = travelTimeMin
                });
            }
        }

        return graph;
    }
}
