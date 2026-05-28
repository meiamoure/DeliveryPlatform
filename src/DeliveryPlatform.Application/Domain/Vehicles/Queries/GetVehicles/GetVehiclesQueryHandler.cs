using MediatR;
using DeliveryPlatform.Application.Domain.Vehicles.Queries.Dtos;
using DeliveryPlatform.Core.Domain.Vehicles.Common;

namespace DeliveryPlatform.Application.Domain.Vehicles.Queries.GetVehicles;

public sealed class GetVehiclesQueryHandler
    : IRequestHandler<GetVehiclesQuery, IReadOnlyList<VehicleDto>>
{
    private readonly IVehicleRepository _vehicles;

    public GetVehiclesQueryHandler(
        IVehicleRepository vehicles)
    {
        _vehicles = vehicles;
    }

    public async Task<IReadOnlyList<VehicleDto>> Handle(
        GetVehiclesQuery query,
        CancellationToken ct)
    {
        var vehicles = await _vehicles.GetAllAsync(ct);

        return vehicles
                    .Select(v => new VehicleDto(
                        v.Id,
                        v.Plate,
                        v.MaxWeightKg,
                        v.MaxVolumeM3,
                        v.SpeedKmh,
                        v.Status.ToString(),
                        v.DepotNodeId,
                        v.DepotNode?.Name,
                        v.CurrentRouteId
                    ))
                    .ToList();
    }
}
