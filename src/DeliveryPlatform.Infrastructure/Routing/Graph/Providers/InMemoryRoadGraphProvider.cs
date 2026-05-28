using System;

namespace DeliveryPlatform.Infrastructure.Routing.Graph.Providers;

public sealed class InMemoryRoadGraphProvider : IRoadGraphProvider
{
    public Task<RoadGraph> GetGraphAsync(CancellationToken cancellationToken)
    {
        var graph = new RoadGraph();

        // Для теста 
        graph.AddEdge(new RoadEdge
        {
            FromNodeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ToNodeId   = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            TravelTimeMin = 10
        });

        graph.AddEdge(new RoadEdge
        {
            FromNodeId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ToNodeId   = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            TravelTimeMin = 7
        });

        return Task.FromResult(graph);
    }
}

