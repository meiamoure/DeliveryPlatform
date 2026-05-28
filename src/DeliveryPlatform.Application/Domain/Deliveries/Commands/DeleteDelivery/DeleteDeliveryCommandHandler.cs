using System;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using MediatR;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Exceptions;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.DeleteDelivery;

public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand, Unit>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeliveryCommandHandler(
        IDeliveryRepository deliveries,
        IUnitOfWork unitOfWork)
    {
        _deliveries = deliveries;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
    {
        var delivery = await _deliveries.GetDeliveryByIdAsync(request.DeliveryId, cancellationToken);

        if (delivery is null)
            throw new ValidationException($"Delivery '{request.DeliveryId}' not found");

        await _deliveries.DeleteAsync(delivery.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
