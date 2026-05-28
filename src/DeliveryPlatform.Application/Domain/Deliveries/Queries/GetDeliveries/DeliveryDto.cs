using System;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Common;

namespace DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveries;

public record DeliveryDto(
    Guid Id,
    string OrderNumber,
    Guid NodeId,
    string NodeName,
    Guid? PickupNodeId,
    decimal WeightKg,
    decimal VolumeM3,
    string ProductGroup,
    DateOnly? ServiceDate,
    TimeOnly? WindowStart,
    TimeOnly? WindowEnd,
    DeliveryPriority Priority,
    DeliveryStatus Status
);
