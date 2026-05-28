using MediatR;

namespace DeliveryPlatform.Application.Domain.Vehicles.Commands.CreateVehicle;

public sealed record CreateVehicleCommand(
    string Plate,
    decimal MaxWeightKg,
    decimal MaxVolumeM3,
    int SpeedKmh,
    Guid DepotNodeId
) : IRequest<Guid>;
