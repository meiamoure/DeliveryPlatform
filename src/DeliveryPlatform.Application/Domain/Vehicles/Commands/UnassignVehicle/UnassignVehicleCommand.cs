using MediatR;

namespace DeliveryPlatform.Application.Domain.Vehicles.Commands.UnassignVehicle;

public sealed record UnassignVehicleCommand(
    Guid VehicleId
) : IRequest;
