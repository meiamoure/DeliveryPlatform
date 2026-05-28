using MediatR;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Common;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.CompleteRoute;
public sealed class CompleteRouteCommandHandler : IRequestHandler<CompleteRouteCommand>
{
    private readonly IRouteRepository _routes;
    private readonly IVehicleRepository _vehicles;
    private readonly IDeliveryRepository _deliveries;
    private readonly IUnitOfWork _uow;

    public CompleteRouteCommandHandler(
        IRouteRepository routes,
        IVehicleRepository vehicles,
        IDeliveryRepository deliveries,
        IUnitOfWork uow)
    {
        _routes = routes;
        _vehicles = vehicles;
        _deliveries = deliveries;
        _uow = uow;
    }

    public async Task Handle(CompleteRouteCommand request, CancellationToken ct)
    {
        var route = await _routes.GetRouteByIdAsync(request.RouteId, ct)
            ?? throw new KeyNotFoundException($"Route {request.RouteId} not found");

        route.Complete();

        var vehicle = await _vehicles.GetVehicleByIdAsync(route.VehicleId, ct)
            ?? throw new KeyNotFoundException($"Vehicle {route.VehicleId} not found");

        vehicle.Unassign();

        var deliveryIds = route.Segments
            .Where(s => s.DeliveryId.HasValue)
            .Select(s => s.DeliveryId!.Value)
            .Distinct()
            .ToList();

        if (deliveryIds.Count > 0)
        {
            var deliveries = await _deliveries.GetDeliveriesByIdsAsync(deliveryIds, ct);

            foreach (var delivery in deliveries)
            {
                delivery.MarkDelivered();
            }
        }

        await _uow.SaveChangesAsync(ct);
    }
}
