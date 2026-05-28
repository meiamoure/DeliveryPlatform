using MediatR;
using DeliveryPlatform.Core.Domain.Vehicles.Models;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Vehicles.Data;

namespace DeliveryPlatform.Application.Domain.Vehicles.Commands.CreateVehicle;

public sealed class CreateVehicleCommandHandler
    : IRequestHandler<CreateVehicleCommand, Guid>
{
    private readonly IVehicleRepository _vehicles;
    private readonly IUnitOfWork _uow;

    public CreateVehicleCommandHandler(
        IVehicleRepository vehicles,
        IUnitOfWork uow)
    {
        _vehicles = vehicles;
        _uow = uow;
    }

    public async Task<Guid> Handle(
        CreateVehicleCommand command,
        CancellationToken ct)
    {
        var vehicle = Vehicle.Create(
            new CreateVehicleData(
                command.Plate,
                command.MaxWeightKg,
                command.MaxVolumeM3,
                command.SpeedKmh,
                command.DepotNodeId));

        await _vehicles.AddAsync(vehicle, ct);

        await _uow.SaveChangesAsync(ct);

        return vehicle.Id;
    }
}
