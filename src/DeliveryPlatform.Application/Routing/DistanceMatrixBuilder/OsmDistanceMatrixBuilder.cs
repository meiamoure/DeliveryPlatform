using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Nodes.Common;

namespace DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;

public sealed class OsmDistanceMatrixBuilder : IDistanceMatrixBuilder
{
    private readonly INodeRepository _nodes;
    private readonly IOsmRoutingService _osrm;

    public OsmDistanceMatrixBuilder(INodeRepository nodes, IOsmRoutingService osrm)
    {
        _nodes = nodes;
        _osrm = osrm;
    }

    public async Task<DistanceMatrix> BuildAsync(
        IReadOnlyCollection<Guid> nodeIds,
        CancellationToken ct)
    {
        var allNodes = await _nodes.GetNodesByIdsAsync(nodeIds, ct);

        var byId = allNodes.ToDictionary(n => n.Id);

        var nodes = nodeIds
            .Select(id => byId[id])
            .ToList();

        var coords = nodes
            .Select(n => (lat: n.Lat, lng: n.Lng))
            .ToList();

        var table = await _osrm.GetTableAsync(coords, ct);

        var data = new Dictionary<(Guid, Guid), DistanceMatrixEntry>();

        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                if (i == j)
                    continue;

                var from = nodes[i].Id;
                var to = nodes[j].Id;

                var durationMin = table.Durations[i][j] / 60.0;
                var distanceKm = table.Distances[i][j] / 1000.0;

                data[(from, to)] = new DistanceMatrixEntry(
                    DistanceKm: Math.Round(distanceKm, 2),
                    TravelTimeMin: (int)Math.Ceiling(durationMin)
                );
            }
        }

        return new DistanceMatrix(data);
    }
}
