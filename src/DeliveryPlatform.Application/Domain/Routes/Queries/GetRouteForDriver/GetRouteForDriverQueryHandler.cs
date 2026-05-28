using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteForDriver;

public sealed class GetRouteForDriverQueryHandler
    : IRequestHandler<GetRouteForDriverQuery, DriverRouteDto?>
{
    private readonly IRouteRepository _routes;
    private readonly IDeliveryRepository _deliveries;
    private readonly INodeRepository _nodes;

    public GetRouteForDriverQueryHandler(IRouteRepository routes, IDeliveryRepository deliveries, INodeRepository nodes)
    {
        _routes = routes;
        _deliveries = deliveries;
        _nodes = nodes;
    }

    public async Task<DriverRouteDto?> Handle(GetRouteForDriverQuery q, CancellationToken ct)
    {
        var routes = await _routes.GetAllAsync(ct);

        var route = routes
            .Where(r =>
                r.DriverId == q.DriverId &&
                r.Status != RouteStatus.Completed
            )
            .OrderByDescending(r => r.ServiceDate)
            .FirstOrDefault();

        if (route is null)
            return null;

        var segs = route.Segments.OrderBy(s => s.Order).ToList();

        var orderedNodeIds = new List<Guid>();
        if (segs.Count > 0)
        {
            orderedNodeIds.Add(segs[0].FromNodeId);
            orderedNodeIds.AddRange(segs.Select(s => s.ToNodeId));
        }

        var depotId = orderedNodeIds.FirstOrDefault();

        var deliveryIds = segs
            .Where(s => s.DeliveryId != null)
            .Select(s => s.DeliveryId!.Value)
            .Distinct()
            .ToList();

        var deliveries = await _deliveries.GetDeliveriesByIdsAsync(deliveryIds, ct);
        var orderNumberByDeliveryId = deliveries.ToDictionary(d => d.Id, d => d.OrderNumber);

        var nodes = await _nodes.GetNodesByIdsAsync(orderedNodeIds, ct);
        var nodesById = nodes.ToDictionary(n => n.Id);

        var stops = orderedNodeIds.Select((nodeId, idx) =>
        {
            var node = nodesById[nodeId];

            var orderNumber = segs
                .FirstOrDefault(s => s.ToNodeId == nodeId && s.DeliveryId != null)?
                .DeliveryId is Guid dId && orderNumberByDeliveryId.ContainsKey(dId)
                    ? orderNumberByDeliveryId[dId]
                    : null;

            return new RouteStopDto(
                Sequence: idx,
                NodeId: node.Id,
                NodeName: node.Name,
                Lat: node.Lat,
                Lng: node.Lng,
                OrderNumber: orderNumber
            );
        }).ToList();

        var orderedPoints = stops
            .Select(s => new RoutePointDto(s.NodeId, s.Lat, s.Lng))
            .ToList();

        var path = orderedPoints
            .Select(p => new RoutePathPointDto(p.Lat, p.Lng))
            .ToList();

        return new DriverRouteDto(
            Id: route.Id,
            VehiclePlate: "—", // пока заглушка
            ServiceDate: route.ServiceDate,
            Status: route.Status.ToString(),

            OrderedPoints: orderedPoints,
            Path: path,
            Stops: stops,

            TotalDistanceKm: route.TotalDistanceKm,
            TotalTimeMin: route.TotalTimeMin,

            Number: route.Number,
            Code: route.Code
        );
    }
}
