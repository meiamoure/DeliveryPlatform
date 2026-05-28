/*using DeliveryPlatform.Infrastructure.Routing.Graph;
using DeliveryPlatform.Infrastructure.Routing.ShortestPath;

namespace DeliveryPlatform.Infrastructure.Tests;

public sealed class TestRoadGraphProvider : IRoadGraphProvider
{
    public Task<RoadGraph> GetGraphAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var graph = new RoadGraph();

        var A = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var B = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var C = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        graph.AddEdge(new RoadEdge { FromNodeId = A, ToNodeId = B, TravelTimeMin = 5 });
        graph.AddEdge(new RoadEdge { FromNodeId = B, ToNodeId = C, TravelTimeMin = 5 });
        graph.AddEdge(new RoadEdge { FromNodeId = A, ToNodeId = C, TravelTimeMin = 15 });

        return Task.FromResult(graph);
    }
}*/
