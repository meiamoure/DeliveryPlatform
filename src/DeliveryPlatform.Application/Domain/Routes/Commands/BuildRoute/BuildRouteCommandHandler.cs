using DeliveryPlatform.Application.Routing.DistanceMatrixBuilder;
using DeliveryPlatform.Application.Routing.Vrp;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Common;
using MediatR;
using DeliveryPlatform.Application.Routing;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Application.Routing.Models;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Domain.Deliveries.Models;
using DeliveryPlatform.Core.Domain.Vehicles.Models;


namespace DeliveryPlatform.Application.Domain.Routes.Commands.BuildRoute;

public sealed class BuildRouteCommandHandler
    : IRequestHandler<BuildRouteCommand, Guid>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly IDistanceMatrixBuilder _matrixBuilder;
    private readonly IVrpSolver _vrpSolver;
    private readonly RouteBuilder _routeBuilder;
    private readonly IRouteRepository _routes;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _uow;

    public BuildRouteCommandHandler(
        IDeliveryRepository deliveries,
        IDistanceMatrixBuilder matrixBuilder,
        IVrpSolver vrpSolver,
        RouteBuilder routeBuilder,
        IRouteRepository routes,
        IVehicleRepository vehicleRepository,
        IUnitOfWork uow)
    {
        _deliveries = deliveries;
        _matrixBuilder = matrixBuilder;
        _vrpSolver = vrpSolver;
        _routeBuilder = routeBuilder;
        _vehicleRepository = vehicleRepository;
        _routes = routes;
        _uow = uow;
    }

    public async Task<Guid> Handle(
        BuildRouteCommand command,
        CancellationToken ct)
    {
        if (command.DeliveryIds == null || command.DeliveryIds.Count == 0)
            throw new ValidationException("At least one delivery must be selected");

        var vehicle = await _vehicleRepository.GetVehicleByIdAsync(command.VehicleId, ct)
    ?? throw new Exception("Vehicle not found");

        if (vehicle == null)
            throw new ValidationException($"Vehicle '{command.VehicleId}' not found");

        if (vehicle.Status != VehicleStatus.Available)
            throw new InvalidOperationException("Vehicle is not available");

        var deliveries = await _deliveries.GetDeliveriesByIdsAsync(command.DeliveryIds, ct);

        if (deliveries.Count != command.DeliveryIds.Count)
            throw new ValidationException("Some deliveries were not found");

        foreach (var delivery in deliveries)
        {
            if (delivery.Status != DeliveryStatus.Pending)
                throw new InvalidOperationException($"Delivery '{delivery.Id}' is not pending");
        }

        ValidateVehicleCapacity(vehicle, deliveries);

        var deliveriesByNode = deliveries
            .GroupBy(d => d.NodeId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var nodeIds = deliveries
            .Select(d => d.NodeId)
            .Distinct()
            .ToList();

        nodeIds.Insert(0, command.DepotNodeId);

        Console.WriteLine("=== NODE IDS FOR MATRIX ===");
        foreach (var id in nodeIds)
            Console.WriteLine(id);

        var matrix = await _matrixBuilder.BuildAsync(nodeIds, ct);

        var initial = _vrpSolver.BuildRoute(
            command.DepotNodeId,
            deliveries,
            matrix);

        var improved = TwoOpt.Improve(initial, matrix);

        var orderedNodes =
            RouteCost.TotalTimeMin(improved, matrix) < RouteCost.TotalTimeMin(initial, matrix)
                ? improved
                : initial;

        if (orderedNodes.First() != command.DepotNodeId)
            throw new InvalidOperationException("Route must start at depot");

        if (orderedNodes.Last() != command.DepotNodeId)
            throw new InvalidOperationException("Route must end at depot");

        Console.WriteLine($"NN time: {RouteCost.TotalTimeMin(initial, matrix)} min");
        Console.WriteLine($"2-opt time: {RouteCost.TotalTimeMin(improved, matrix)} min");

        var lastNumber = await _routes.GetLastNumberAsync(ct);
        var newNumber = lastNumber + 1;

        var year = DateTime.UtcNow.Year;
        var code = $"RT-{year}-{newNumber:D5}";

        var route = _routeBuilder.Create(
            orderedNodes,
            matrix,
            vehicle,
            command.Date,
            deliveriesByNode,
            vehicle.DriverId
        );

        route.SetNumber(newNumber);
        route.SetCode(code);

        vehicle.AssignToRoute(route.Id);

        await _routes.AddAsync(route, ct);
        await _uow.SaveChangesAsync(ct);

        foreach (var nodeId in orderedNodes)
        {
            if (!deliveriesByNode.TryGetValue(nodeId, out var nodeDeliveries))
                continue;

            foreach (var delivery in nodeDeliveries)
            {
                if (delivery.Status != DeliveryStatus.Pending)
                    throw new InvalidOperationException($"Delivery {delivery.Id} already assigned");

                delivery.AssignToDate(command.Date);
                delivery.MarkPlanned();
            }
        }

        return route.Id;
    }

    private static void ValidateVehicleCapacity(
        Vehicle vehicle,
        IReadOnlyCollection<Delivery> deliveries)
    {
        var totalWeight = deliveries.Sum(d => d.WeightKg);
        var totalVolume = deliveries.Sum(d => d.VolumeM3);

        if (totalWeight > vehicle.MaxWeightKg)
            throw new ValidationException(
                $"Selected deliveries exceed vehicle weight capacity. " +
                $"Total weight: {totalWeight}, max: {vehicle.MaxWeightKg}");

        if (totalVolume > vehicle.MaxVolumeM3)
            throw new ValidationException(
                $"Selected deliveries exceed vehicle volume capacity. " +
                $"Total volume: {totalVolume}, max: {vehicle.MaxVolumeM3}");
    }
}
