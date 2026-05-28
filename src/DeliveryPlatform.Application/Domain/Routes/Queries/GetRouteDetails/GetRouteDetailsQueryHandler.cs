using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Application.Routing;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteDetails;

public sealed class GetRouteDetailsQueryHandler
    : IRequestHandler<GetRouteDetailsQuery, RouteDetailsDto>
{
    private readonly IRouteRepository _routes;
    private readonly INodeRepository _nodes;

    private readonly IVehicleRepository _vehicle;

    private readonly IOsmRoutingService _osrm;
    private readonly IDeliveryRepository _deliveries;
    private readonly DeliveryPlatformDbContext _dbContext;

    public GetRouteDetailsQueryHandler(IRouteRepository routes, INodeRepository nodes, IVehicleRepository vehicle, IOsmRoutingService osrm, IDeliveryRepository deliveries, DeliveryPlatformDbContext dbContext)
    {
        _routes = routes;
        _nodes = nodes;
        _vehicle = vehicle;
        _osrm = osrm;
        _deliveries = deliveries;
        _dbContext = dbContext;
    }

    public async Task<RouteDetailsDto> Handle(GetRouteDetailsQuery query, CancellationToken ct)
    {
        var route = await _routes.GetRouteByIdAsync(query.RouteId, ct)
            ?? throw new KeyNotFoundException($"Route {query.RouteId} not found");

        var segments = route.Segments
            .OrderBy(s => s.Order)
            .ToList();

        // Собираем порядок узлов: From первого сегмента + To каждого сегмента
        var orderedNodeIds = new List<Guid>(segments.Count + 1);
        if (segments.Count > 0)
        {
            orderedNodeIds.Add(segments[0].FromNodeId);
            orderedNodeIds.AddRange(segments.Select(s => s.ToNodeId));
        }

        // Грузим координаты нужных нод
        var uniqueNodeIds = orderedNodeIds.Distinct().ToList();
        var nodes = await _nodes.GetNodesByIdsAsync(uniqueNodeIds, ct);
        var byId = nodes.ToDictionary(n => n.Id);

        var orderedPoints = orderedNodeIds.Select(id =>
        {
            var n = byId[id];
            return new RoutePointDto(id, n.Lat, n.Lng);
        }).ToList();

        var geometryCoords = orderedPoints
            .Select(p => (lat: p.Lat, lng: p.Lng))
            .ToList();

        var pathCoords = await _osrm.GetRouteGeometryAsync(geometryCoords, ct);

        var path = pathCoords
            .Select(p => new RoutePathPointDto(p.lat, p.lng))
            .ToList();

        var segmentDtos = segments.Select(s => new RouteSegmentDetailsDto(
            Order: s.Order,
            FromNodeId: s.FromNodeId,
            FromNodeName: byId[s.FromNodeId].Name,
            ToNodeId: s.ToNodeId,
            ToNodeName: byId[s.ToNodeId].Name,
            DistanceKm: s.DistanceKm,
            TravelTimeMin: s.TravelTimeMin,
            DeliveryId: s.DeliveryId
        )).ToList();

        var vehicle = await _vehicle.GetVehicleByIdAsync(route.VehicleId, ct)
    ?? throw new KeyNotFoundException($"Vehicle {route.VehicleId} not found");

        var deliveryIds = segments
    .Where(s => s.DeliveryId.HasValue)
    .Select(s => s.DeliveryId!.Value)
    .Distinct()
    .ToList();

        var deliveries = deliveryIds.Count > 0
            ? await _deliveries.GetDeliveriesByIdsAsync(deliveryIds, ct)
            : [];

        var deliveriesByNodeId = deliveries
            .GroupBy(d => d.NodeId)
            .ToDictionary(g => g.Key, g => g.First());

        var stops = orderedNodeIds
            .Select((nodeId, index) =>
            {
                var node = byId[nodeId];

                deliveriesByNodeId.TryGetValue(nodeId, out var delivery);

                return new RouteStopDto(
                    NodeId: node.Id,
                    NodeName: node.Name,
                    OrderNumber: delivery?.OrderNumber,
                    Lat: node.Lat,
                    Lng: node.Lng,
                    Sequence: index
                );
            })
            .ToList();

        var driver = route.DriverId != null
            ? await _dbContext.Drivers.FindAsync(route.DriverId)
            : null;

        return new RouteDetailsDto(
            route.Id,
            route.VehicleId,
            vehicle.Plate,
            route.DriverId,
            driver?.Name,
            route.ServiceDate,
            route.Status.ToString(),
            orderedPoints,
            segmentDtos,
            path,
            stops,
            route.TotalDistanceKm,
            route.TotalTimeMin,
            route.Number,
            route.Code
        );
    }
}
