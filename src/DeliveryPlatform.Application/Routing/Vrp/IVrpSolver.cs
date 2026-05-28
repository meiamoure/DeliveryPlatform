using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Deliveries.Models;


namespace DeliveryPlatform.Application.Routing.Vrp;

public interface IVrpSolver
{
    IReadOnlyList<Guid> BuildRoute(
    Guid depotId,
    IReadOnlyList<Delivery> deliveries,
    DistanceMatrix matrix);
}
