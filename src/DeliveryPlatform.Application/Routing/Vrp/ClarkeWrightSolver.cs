using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Application.Routing.Vrp;

public sealed class ClarkeWrightSolver : IVrpSolver
{
    public IReadOnlyList<Guid> BuildRoute(
        Guid depotId,
        IReadOnlyList<Delivery> deliveries,
        DistanceMatrix matrix)
    {
        // MVP: пока используем Nearest Neighbor
        return new NearestNeighborSolver()
            .BuildRoute(depotId, deliveries, matrix);
    }
}
