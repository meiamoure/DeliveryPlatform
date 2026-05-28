using System;

namespace DeliveryPlatform.Api.Controllers.Deliveries.Requests;

public sealed record CreateDeliveryRequest(
    string Name,
    Guid? PickupNodeId,
    decimal WeightKg,
    decimal VolumeM3,
    int ProductGroup,
    TimeOnly? WindowStart,
    TimeOnly? WindowEnd,
    int Priority
);
