using MediatR;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Application.Domain.Vehicles.Commands.UnassignVehicle;

public sealed class UnassignVehicleCommandHandler
    : IRequestHandler<UnassignVehicleCommand>
{
    private readonly IVehicleRepository _vehicles;
    private readonly IUnitOfWork _uow;

    public UnassignVehicleCommandHandler(
        IVehicleRepository vehicles,
        IUnitOfWork uow)
    {
        _vehicles = vehicles;
        _uow = uow;
    }

    public async Task Handle(
        UnassignVehicleCommand command,
        CancellationToken ct)
    {
        var vehicle = await _vehicles.GetVehicleByIdAsync(command.VehicleId, ct);

        if (vehicle == null)
            throw new Exception("Vehicle not found");

        vehicle.Unassign();

        await _uow.SaveChangesAsync(ct);
    }
}
