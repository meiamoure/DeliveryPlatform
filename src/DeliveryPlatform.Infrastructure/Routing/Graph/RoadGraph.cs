using System;

namespace DeliveryPlatform.Infrastructure.Routing.Graph;

public sealed class RoadGraph
{
    private readonly Dictionary<Guid, List<RoadEdge>> _adjacency = new();

    public void AddEdge(RoadEdge edge)
    {
        if (!_adjacency.ContainsKey(edge.FromNodeId))
            _adjacency[edge.FromNodeId] = new List<RoadEdge>();

        _adjacency[edge.FromNodeId].Add(edge);
    }

    public IEnumerable<Guid> Nodes
{
    get
    {
        var fromNodes = _adjacency.Keys;
        var toNodes = _adjacency.Values.SelectMany(edges => edges.Select(e => e.ToNodeId));
        return fromNodes.Concat(toNodes).Distinct();
    }
}

    public IEnumerable<RoadEdge> GetEdgesFrom(Guid nodeId)
        => _adjacency.TryGetValue(nodeId, out var edges)
            ? edges
            : Enumerable.Empty<RoadEdge>();
}
