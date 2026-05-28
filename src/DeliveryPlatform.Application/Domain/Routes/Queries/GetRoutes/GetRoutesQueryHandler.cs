using System;
using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Common;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.GetRoutes;

public sealed class GetRoutesQueryHandler
    : IRequestHandler<GetRoutesQuery, IReadOnlyList<RouteListItemDto>>
{
    private readonly IRouteRepository _routes;
    private readonly IVehicleRepository _vehicleRepository;

    public GetRoutesQueryHandler(IRouteRepository routes, IVehicleRepository vehicleRepository)
    {
        _routes = routes;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<IReadOnlyList<RouteListItemDto>> Handle(GetRoutesQuery query, CancellationToken ct)
    {
        var routes = await _routes.GetAllAsync(ct);

        if (query.From is not null)
            routes = routes.Where(r => r.ServiceDate >= query.From.Value).ToList();

        if (query.To is not null)
            routes = routes.Where(r => r.ServiceDate <= query.To.Value).ToList();

        var vehicleIds = routes.Select(r => r.VehicleId).Distinct().ToList();
        var vehicles = await _vehicleRepository.GetVehiclesByIdsAsync(vehicleIds, ct);
        var vehicleById = vehicles.ToDictionary(v => v.Id);

        return routes
            .Select(r => new RouteListItemDto(
                Id: r.Id,
                VehicleId: r.VehicleId,
                VehiclePlate: vehicleById.TryGetValue(r.VehicleId, out var vehicle)
                ? vehicle.Plate
                : "Невідомо",
                DriverId: r.DriverId,
                ServiceDate: r.ServiceDate,
                Status: r.Status.ToString(),
                SegmentsCount: r.Segments.Count,
                TotalDistanceKm: r.TotalDistanceKm,
                TotalTimeMin: r.TotalTimeMin,
                Number: r.Number,
                Code: r.Code
            ))
            .OrderByDescending(x => x.ServiceDate)
            .ToList();
    }
}
