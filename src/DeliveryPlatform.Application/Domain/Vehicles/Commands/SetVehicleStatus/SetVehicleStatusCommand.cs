using MediatR;
using DeliveryPlatform.Core.Domain.Vehicles.Common;

namespace DeliveryPlatform.Application.Domain.Vehicles.Commands.SetVehicleStatus;

public sealed record SetVehicleStatusCommand(
    Guid VehicleId,
    VehicleStatus Status
) : IRequest;
