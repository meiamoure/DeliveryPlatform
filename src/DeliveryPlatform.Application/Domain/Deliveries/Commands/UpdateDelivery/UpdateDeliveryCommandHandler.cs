using System.Reflection;
using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Models;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Exceptions;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Application.Common.Geocoding;
using DeliveryPlatform.Core.Domain.Nodes.Models;
using DeliveryPlatform.Core.Domain.Nodes.Data;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.UpdateDelivery;

public sealed class UpdateDeliveryCommandHandler : IRequestHandler<UpdateDeliveryCommand, Unit>
{
    private readonly IDeliveryRepository _deliveries;
    private readonly IUnitOfWork _uow;

    private readonly INodeRepository _nodeRepository;
    private readonly IGeocodingService _geocoding;
    private readonly Dictionary<string, Node> _nodeCache = new();

    public UpdateDeliveryCommandHandler(IDeliveryRepository deliveries, IUnitOfWork uow, INodeRepository nodeRepository, IGeocodingService geocoding)
    {
        _deliveries = deliveries;
        _uow = uow;
        _nodeRepository = nodeRepository;
        _geocoding = geocoding;
    }

    public async Task<Unit> Handle(UpdateDeliveryCommand request, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var delivery = await _deliveries.GetDeliveryByIdAsync(request.Id, ct);
        if (delivery is null)
            throw new ValidationException($"Delivery with id '{request.Id}' not found");

        Guid? nodeId = null;

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var node = await GetOrCreateNodeAsync(request.Name, ct);
            nodeId = node.Id;
        }

        delivery.Update(
            null,
            nodeId,
            request.PickupNodeId,
            request.WeightKg,
            request.VolumeM3,
            request.ProductGroup,
            request.WindowStart,
            request.WindowEnd,
            request.Priority
        );

        await _deliveries.UpdateAsync(delivery, ct);
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }

    private async Task<Node> GetOrCreateNodeAsync(string name, CancellationToken ct)
    {
        // 1. Проверка кэша
        if (_nodeCache.TryGetValue(name, out var cached))
            return cached;

        // 2. Проверка базы
        var node = await _nodeRepository.GetByNameAsync(name, ct);

        if (node != null)
        {
            _nodeCache[name] = node;
            return node;
        }

        // 3. Nominatim
        var (lat, lng) = await _geocoding.GetCoordinatesAsync(name);

        node = Node.Create(new CreateNodeData(
            Name: name,
            Lat: lat,
            Lng: lng,
            Type: NodeType.Client
        ));

        await _nodeRepository.AddAsync(node, ct);

        _nodeCache[name] = node;

        return node;
    }
}
