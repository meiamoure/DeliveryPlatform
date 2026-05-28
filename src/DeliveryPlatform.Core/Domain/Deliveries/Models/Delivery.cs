using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Data;
using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Nodes.Models;

namespace DeliveryPlatform.Core.Domain.Deliveries.Models;

public class Delivery : IAggregateRoot
{
    private Delivery() { }

    internal Delivery(
        Guid id,
        string orderNumber,
        Guid nodeId,
        decimal weightKg,
        decimal volumeM3,
        ProductGroup productGroup,
        DateOnly? serviceDate,
        TimeOnly? windowStart,
        TimeOnly? windowEnd,
        DeliveryPriority priority,
        DeliveryStatus status,
        Guid? pickupNodeId)
    {
        Id = id;
        OrderNumber = orderNumber;
        NodeId = nodeId;
        WeightKg = weightKg;
        VolumeM3 = volumeM3;
        ProductGroup = productGroup;
        ServiceDate = serviceDate;
        WindowStart = windowStart;
        WindowEnd = windowEnd;
        Priority = priority;
        Status = status;
        PickupNodeId = pickupNodeId;
    }

    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = default!;
    public Guid NodeId { get; private set; }
    public Guid? PickupNodeId { get; private set; }

    // Новые поля
    public decimal WeightKg { get; private set; }
    public decimal VolumeM3 { get; private set; }
    public ProductGroup ProductGroup { get; private set; }

    public DateOnly? ServiceDate { get; private set; }
    public TimeOnly? WindowStart { get; private set; }
    public TimeOnly? WindowEnd { get; private set; }

    public DeliveryPriority Priority { get; private set; }
    public DeliveryStatus Status { get; private set; }

    public Node Node { get; private set; } = null!;

    public static Delivery Create(CreateDeliveryData d) =>
        new(
            Guid.NewGuid(),
            d.OrderNumber,
            d.NodeId,
            d.WeightKg,
            d.VolumeM3,
            d.ProductGroup,
            null,
            d.WindowStart,
            d.WindowEnd,
            d.Priority,
            DeliveryStatus.Pending,
            d.PickupNodeId
        );

    public void Update(
    string? orderNumber,
    Guid? nodeId,
    Guid? pickupNodeId,
    decimal? weightKg,
    decimal? volumeM3,
    ProductGroup? productGroup,
    TimeOnly? windowStart,
    TimeOnly? windowEnd,
    DeliveryPriority? priority)
    {
        if (orderNumber != null) OrderNumber = orderNumber;
        if (nodeId != null) NodeId = nodeId.Value;
        if (pickupNodeId != null) PickupNodeId = pickupNodeId;
        if (weightKg != null) WeightKg = weightKg.Value;
        if (volumeM3 != null) VolumeM3 = volumeM3.Value;
        if (productGroup != null) ProductGroup = productGroup.Value;
        if (windowStart != null) WindowStart = windowStart.Value;
        if (windowEnd != null) WindowEnd = windowEnd.Value;
        if (priority != null) Priority = priority.Value;
    }

    public void AssignToDate(DateOnly date)
    {
        ServiceDate = date;
    }

    public void MarkPlanned()
    {
        if (Status != DeliveryStatus.Pending)
            throw new InvalidOperationException("Only pending delivery can be planned");

        Status = DeliveryStatus.Planned;
    }

    public void MarkDelivered()
    {
        if (Status == DeliveryStatus.Cancelled)
            throw new InvalidOperationException("Cancelled delivery cannot be marked as delivered");

        Status = DeliveryStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == DeliveryStatus.Delivered)
            throw new InvalidOperationException("Delivered delivery cannot be cancelled");

        Status = DeliveryStatus.Cancelled;
    }
}

