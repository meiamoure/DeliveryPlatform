using System;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Core.Common;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Vehicles.Commands.SetVehicleStatus;

public sealed class SetVehicleStatusCommandHandler : IRequestHandler<SetVehicleStatusCommand>
{
    private readonly IVehicleRepository _vehicles;
    private readonly IUnitOfWork _unitOfWork;

    public SetVehicleStatusCommandHandler(
        IVehicleRepository vehicles,
        IUnitOfWork unitOfWork)
    {
        _vehicles = vehicles;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SetVehicleStatusCommand request, CancellationToken ct)
    {
        var vehicle = await _vehicles.GetVehicleByIdAsync(request.VehicleId, ct);

        if (vehicle is null)
        {
            throw new KeyNotFoundException($"Vehicle with id '{request.VehicleId}' was not found.");
        }

        vehicle.SetStatus(request.Status);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
