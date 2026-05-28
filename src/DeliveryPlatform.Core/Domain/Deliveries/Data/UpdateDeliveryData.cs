using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Core.Domain.Deliveries.Data;

public sealed record UpdateDeliveryData(
    string OrderNumber,
    Guid NodeId,
    int Load,
    decimal WeightKg,
    decimal VolumeM3,
    ProductGroup ProductGroup,
    TimeOnly? WindowStart,
    TimeOnly? WindowEnd,
    DeliveryPriority Priority,
    Guid? PickupNodeId
);
