using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveryById;

public sealed class GetDeliveryByIdQueryHandler : IRequestHandler<GetDeliveryByIdQuery, Delivery?>
{
    private readonly IDeliveryRepository _deliveries;

    public GetDeliveryByIdQueryHandler(IDeliveryRepository deliveries)
    {
        _deliveries = deliveries;
    }

    public async Task<Delivery?> Handle(GetDeliveryByIdQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _deliveries.GetDeliveryByIdAsync(request.Id, cancellationToken);
    }
}
