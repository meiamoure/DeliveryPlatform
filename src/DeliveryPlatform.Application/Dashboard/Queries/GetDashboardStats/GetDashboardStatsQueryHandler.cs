using System;
using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Common;

namespace DeliveryPlatform.Application.Dashboard.Queries.GetDashboardStats;

public sealed class GetDashboardStatsQueryHandler
    : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly IRouteRepository _routes;
    private readonly IVehicleRepository _vehicles;

    public GetDashboardStatsQueryHandler(
        IDeliveryRepository deliveries,
        IRouteRepository routes,
        IVehicleRepository vehicles)
    {
        _deliveries = deliveries;
        _routes = routes;
        _vehicles = vehicles;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery q, CancellationToken ct)
    {
        var pending = await _deliveries.GetPendingAsync(ct);
        var allRoutes = await _routes.GetAllAsync(ct);
        var allVehicles = await _vehicles.GetAllAsync(ct);

        var plannedRoutesForDate = allRoutes.Count(r => r.ServiceDate == q.Date);

        return new DashboardStatsDto(
            PendingDeliveriesCount: pending.Count,
            VehiclesCount: allVehicles.Count,
            PlannedRoutesCount: plannedRoutesForDate
        );
    }
}
