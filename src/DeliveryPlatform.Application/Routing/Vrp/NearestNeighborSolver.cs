using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Application.Routing.Vrp;

public sealed class NearestNeighborSolver : IVrpSolver
{
    public IReadOnlyList<Guid> BuildRoute(
        Guid depotId,
        IReadOnlyList<Delivery> deliveries,
        DistanceMatrix matrix)
    {
        var deliveryNodes = deliveries.Select(d => d.NodeId).Distinct().ToList();

        ValidateMatrixOrThrow(depotId, deliveryNodes, matrix);

        var route = new List<Guid>(deliveryNodes.Count + 2) { depotId };
        var remaining = new HashSet<Guid>(deliveryNodes);

        var current = depotId;

        while (remaining.Count > 0)
        {
            Guid best = default;
            var bestTime = int.MaxValue;
            var found = false;

            foreach (var candidate in remaining)
            {
                var t = matrix.Get(current, candidate).TravelTimeMin;

                if (t < bestTime)
                {
                    bestTime = t;
                    best = candidate;
                    found = true;
                }
            }

            if (!found)
                throw new InvalidOperationException($"No next node found from {current}.");

            route.Add(best);
            remaining.Remove(best);
            current = best;
        }

        route.Add(depotId);
        return route;
    }

    private static void ValidateMatrixOrThrow(
        Guid depotId,
        IReadOnlyList<Guid> nodes,
        DistanceMatrix matrix)
    {
        var all = new List<Guid>(nodes.Count + 1);
        all.Add(depotId);
        all.AddRange(nodes);

        foreach (var from in all)
        foreach (var to in all)
        {
            if (from == to) continue;

            try
            {
                _ = matrix.Get(from, to);
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException(
                    $"Matrix is incomplete: missing entry {from} -> {to}. " +
                    $"Make sure OSRM table/matrix generation includes depot + all delivery nodes.");
            }
        }
    }
}
