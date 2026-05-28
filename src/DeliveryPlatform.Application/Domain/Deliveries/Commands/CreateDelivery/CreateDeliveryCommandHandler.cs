using System;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Models;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Data;
using MediatR;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Application.Common.Geocoding;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Core.Domain.Nodes.Models;
using DeliveryPlatform.Core.Domain.Nodes.Data;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.CreateDelivery;

public sealed class CreateDeliveryCommandHandler 
    : IRequestHandler<CreateDeliveryCommand, Guid>
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUnitOfWork _uow;
    private readonly IGeocodingService _geocoding;
    private readonly INodeRepository _nodeRepository;

    public CreateDeliveryCommandHandler(
        IDeliveryRepository deliveryRepository,
        IUnitOfWork uow,
        IGeocodingService geocoding,
        INodeRepository nodeRepository)
    {
        _deliveryRepository = deliveryRepository;
        _uow = uow;
        _geocoding = geocoding;
        _nodeRepository = nodeRepository;
    }

    public async Task<Guid> Handle(CreateDeliveryCommand request, CancellationToken ct)
    {
        if (request.WindowStart is null || request.WindowEnd is null)
            throw new ValidationException("Delivery time window is required");

        if (request.WindowEnd <= request.WindowStart)
            throw new ValidationException("WindowEnd must be after WindowStart");

        var node = await GetOrCreateNodeAsync(request.Name, ct);

        var number = await _deliveryRepository.GetNextDeliveryNumber(ct);
        var orderNumber = $"D-{number:D3}";

        var delivery = Delivery.Create(
            new CreateDeliveryData(
                OrderNumber: orderNumber,
                NodeId: node.Id,
                WeightKg: request.WeightKg,
                VolumeM3: request.VolumeM3,
                ProductGroup: request.ProductGroup,
                WindowStart: request.WindowStart.Value,
                WindowEnd: request.WindowEnd.Value,
                Priority: request.Priority,
                PickupNodeId: request.PickupNodeId
            )
        );

        await _deliveryRepository.AddAsync(delivery, ct);
        await _uow.SaveChangesAsync(ct);

        return delivery.Id;
    }

    private async Task<Node> GetOrCreateNodeAsync(string name, CancellationToken ct)
    {
        var node = await _nodeRepository.GetByNameAsync(name, ct);

        if (node != null)
            return node;

        var (lat, lng) = await _geocoding.GetCoordinatesAsync(name);

        node = Node.Create(new CreateNodeData(
            Name: name,
            Lat: lat,
            Lng: lng,
            Type: NodeType.Client
        ));

        await _nodeRepository.AddAsync(node, ct);

        return node;
    }
}
