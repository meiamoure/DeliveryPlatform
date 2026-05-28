using System;
using System.Collections;

namespace DeliveryPlatform.Application.Routing.ShortestPath;

public static class PathRestorer
{
    public static IReadOnlyList<Guid> RestorePath(
        Guid targetNodeId,
        IReadOnlyDictionary<Guid, Guid?> parents)
    {
        var path = new List<Guid>();
        var current = targetNodeId;

        while (parents.TryGetValue(current, out var parent) && parent != null)
        {
            path.Add(current);
            current = parent.Value;
        }

        path.Add(current);
        path.Reverse();

        return path;
    }
}
