using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Application.Routing.Vrp;
using DeliveryPlatform.Application.Routing;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Domain.Vehicles.Models;
using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.BuildManyRoutes;


public sealed class BuildManyRoutesCommandHandler
    : IRequestHandler<BuildManyRoutesCommand, BuildManyRoutesResult>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IDistanceMatrixBuilder _matrixBuilder;
    private readonly IVrpSolver _vrpSolver;
    private readonly RouteBuilder _routeBuilder;
    private readonly IRouteRepository _routes;
    private readonly IUnitOfWork _uow;

    public BuildManyRoutesCommandHandler(
        IDeliveryRepository deliveries,
        IVehicleRepository vehicleRepository,
        IDistanceMatrixBuilder matrixBuilder,
        IVrpSolver vrpSolver,
        RouteBuilder routeBuilder,
        IRouteRepository routes,
        IUnitOfWork uow)
    {
        _deliveries = deliveries;
        _vehicleRepository = vehicleRepository;
        _matrixBuilder = matrixBuilder;
        _vrpSolver = vrpSolver;
        _routeBuilder = routeBuilder;
        _routes = routes;
        _uow = uow;
    }

    public async Task<BuildManyRoutesResult> Handle(
         BuildManyRoutesCommand command,
         CancellationToken ct)
    {
        if (command.VehicleIds == null || command.VehicleIds.Count == 0)
            throw new ValidationException("At least one vehicle must be selected");

        if (command.DeliveryIds == null || command.DeliveryIds.Count == 0)
            throw new ValidationException("At least one delivery must be selected");

        var vehicles = await _vehicleRepository.GetVehiclesByIdsAsync(command.VehicleIds, ct);
        var deliveries = await _deliveries.GetDeliveriesByIdsAsync(command.DeliveryIds, ct);

        if (vehicles.Count != command.VehicleIds.Count)
            throw new ValidationException("Some vehicles were not found");

        if (deliveries.Count != command.DeliveryIds.Count)
            throw new ValidationException("Some deliveries were not found");

        foreach (var vehicle in vehicles)
        {
            if (vehicle.Status != VehicleStatus.Available)
                throw new InvalidOperationException($"Vehicle '{vehicle.Id}' is not available");
        }

        foreach (var delivery in deliveries)
        {
            if (delivery.Status != DeliveryStatus.Pending)
                throw new InvalidOperationException($"Delivery '{delivery.Id}' is not pending");
        }

        var remainingDeliveries = deliveries
            .OrderByDescending(d => d.WeightKg)
            .ThenByDescending(d => d.VolumeM3)
            .ToList();

        var builtRoutes = new List<BuildManyRouteItemResult>();

        foreach (var vehicle in vehicles)
        {
            var deliveriesForVehicle = SelectDeliveriesForVehicle(vehicle, remainingDeliveries);

            if (deliveriesForVehicle.Count == 0)
                continue;

            var routeId = await BuildRouteForVehicleAsync(
                command.DepotNodeId,
                vehicle,
                deliveriesForVehicle,
                command.Date,
                ct);

            builtRoutes.Add(new BuildManyRouteItemResult(
                RouteId: routeId,
                VehicleId: vehicle.Id,
                VehiclePlate: vehicle.Plate,
                DeliveriesCount: deliveriesForVehicle.Count
            ));

            var assignedIds = deliveriesForVehicle
                .Select(d => d.Id)
                .ToHashSet();

            remainingDeliveries = remainingDeliveries
                .Where(d => !assignedIds.Contains(d.Id))
                .ToList();
        }

        await _uow.SaveChangesAsync(ct);

        return new BuildManyRoutesResult(
            Routes: builtRoutes,
            UnassignedDeliveryIds: remainingDeliveries
                .Select(d => d.Id)
                .ToList()
        );
    }

    private async Task<Guid> BuildRouteForVehicleAsync(
        Guid depotNodeId,
        Vehicle vehicle,
        IReadOnlyList<Delivery> deliveries,
        DateOnly date,
        CancellationToken ct)
    {
        var deliveriesByNode = deliveries
            .GroupBy(d => d.NodeId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var nodeIds = deliveries
            .Select(d => d.NodeId)
            .Distinct()
            .ToList();

        nodeIds.Insert(0, depotNodeId);

        Console.WriteLine($"=== BUILD ROUTE FOR VEHICLE {vehicle.Id} ===");
        foreach (var nodeId in nodeIds)
            Console.WriteLine(nodeId);

        var matrix = await _matrixBuilder.BuildAsync(nodeIds, ct);

        var initial = _vrpSolver.BuildRoute(
            depotNodeId,
            deliveries,
            matrix);

        var improved = TwoOpt.Improve(initial, matrix);

        var orderedNodes =
            RouteCost.TotalTimeMin(improved, matrix) < RouteCost.TotalTimeMin(initial, matrix)
                ? improved
                : initial;

        var activeRoute = await _routes.GetActiveByVehicleIdAsync(vehicle.Id, ct);

        if (activeRoute != null)
            throw new InvalidOperationException("Vehicle already has active route");

        if (orderedNodes.First() != depotNodeId)
            throw new InvalidOperationException("Route must start at depot");

        if (orderedNodes.Last() != depotNodeId)
            throw new InvalidOperationException("Route must end at depot");

        if (vehicle.DriverId == Guid.Empty)
            throw new InvalidOperationException("Vehicle has no driver");

        var lastNumber = await _routes.GetLastNumberAsync(ct);
        var newNumber = lastNumber + 1;

        var year = DateTime.UtcNow.Year;
        var code = $"RT-{year}-{newNumber:D5}";

        var route = _routeBuilder.Create(
            orderedNodes,
            matrix,
            vehicle,
            date,
            deliveriesByNode,
            vehicle.DriverId
        );

        route.SetNumber(newNumber);
        route.SetCode(code);

        vehicle.AssignToRoute(route.Id);

        await _routes.AddAsync(route, ct);

        foreach (var nodeId in orderedNodes)
        {
            if (!deliveriesByNode.TryGetValue(nodeId, out var nodeDeliveries))
                continue;

            foreach (var delivery in nodeDeliveries)
            {
                if (delivery.Status != DeliveryStatus.Pending)
                    throw new InvalidOperationException($"Delivery {delivery.Id} already assigned");

                delivery.AssignToDate(date);
                delivery.MarkPlanned();
            }
        }

        return route.Id;
    }

    private static List<Delivery> SelectDeliveriesForVehicle(
        Vehicle vehicle,
        IEnumerable<Delivery> deliveries)
    {
        var result = new List<Delivery>();

        decimal totalWeight = 0;
        decimal totalVolume = 0;

        foreach (var delivery in deliveries)
        {
            if (totalWeight + delivery.WeightKg > vehicle.MaxWeightKg)
                continue;

            if (totalVolume + delivery.VolumeM3 > vehicle.MaxVolumeM3)
                continue;

            result.Add(delivery);

            totalWeight += delivery.WeightKg;
            totalVolume += delivery.VolumeM3;
        }

        return result;
    }
}
