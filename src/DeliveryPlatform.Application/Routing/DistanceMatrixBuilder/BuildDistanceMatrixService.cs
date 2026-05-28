using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Application.Routing.ShortestPath;
using DeliveryPlatform.Core.Domain.Nodes.Common;

namespace DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;

/*public sealed class BuildDistanceMatrixService : IDistanceMatrixBuilder
{
    private readonly IShortestPathService _shortestPath;

    public BuildDistanceMatrixService(IShortestPathService shortestPath)
    {
        _shortestPath = shortestPath;
    }

    public async Task<DistanceMatrix> BuildAsync(
        IReadOnlyCollection<Guid> nodeIds,
        CancellationToken ct)
    {
        var data = new Dictionary<(Guid, Guid), DistanceMatrixEntry>();

        foreach (var from in nodeIds)
        {
            var result = await _shortestPath.RunFromAsync(from, ct);

            foreach (var to in nodeIds)
            {
                if (from == to) continue;

                data[(from, to)] = new DistanceMatrixEntry(
                    DistanceKm: result.Distances[to] / 1000.0,
                    TravelTimeMin: result.Distances[to]
                );
            }
        }

        return new DistanceMatrix(data);
    }
}*/
