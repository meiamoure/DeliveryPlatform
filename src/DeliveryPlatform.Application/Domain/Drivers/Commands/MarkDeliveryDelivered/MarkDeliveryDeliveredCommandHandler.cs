using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Exceptions;

namespace DeliveryPlatform.Application.Domain.Drivers.Commands.MarkDeliveryDelivered;

public class MarkDeliveryDeliveredCommandHandler : IRequestHandler<MarkDeliveryDeliveredCommand, Unit>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly IUnitOfWork _uow;

    public MarkDeliveryDeliveredCommandHandler(
        IDeliveryRepository deliveries,
        IUnitOfWork uow)
    {
        _deliveries = deliveries;
        _uow = uow;
    }

    public async Task<Unit> Handle(MarkDeliveryDeliveredCommand request, CancellationToken ct)
    {
        var delivery = await _deliveries.GetDeliveryByIdAsync(request.Id, ct);

        if (delivery is null)
            throw new ValidationException($"Delivery '{request.Id}' not found");

        delivery.MarkDelivered();

        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}



