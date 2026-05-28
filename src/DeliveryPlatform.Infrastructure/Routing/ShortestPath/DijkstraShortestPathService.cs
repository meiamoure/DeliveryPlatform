using DeliveryPlatform.Application.Routing.ShortestPath;
using DeliveryPlatform.Infrastructure.Routing.Graph;

namespace DeliveryPlatform.Infrastructure.Routing.ShortestPath;

/*public sealed class DijkstraShortestPathService : IShortestPathService
{
    private readonly IRoadGraphProvider _graphProvider;

    public DijkstraShortestPathService(IRoadGraphProvider graphProvider)
    {
        _graphProvider = graphProvider;
    }

    public async Task<ShortestPathResult> RunFromAsync(
        Guid sourceNodeId,
        CancellationToken ct)
    {
        var graph = await _graphProvider.GetGraphAsync(ct);

        var dist = new Dictionary<Guid, int>();
        var parent = new Dictionary<Guid, Guid?>();

        var queue = new PriorityQueue<Guid, int>();

        foreach (var node in graph.Nodes)
        {
            dist[node] = int.MaxValue;
            parent[node] = null;
        }

        dist[sourceNodeId] = 0;
        queue.Enqueue(sourceNodeId, 0);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var currentDist = dist[current];

            foreach (var edge in graph.GetEdgesFrom(current))
            {
                var candidate = currentDist + edge.TravelTimeMin;

                if (candidate < dist[edge.ToNodeId])
                {
                    dist[edge.ToNodeId] = candidate;
                    parent[edge.ToNodeId] = current;
                    queue.Enqueue(edge.ToNodeId, candidate);
                }
            }
        }

        return new ShortestPathResult
        {
            Distances = dist,
            Parents = parent
        };
    }
}*/
