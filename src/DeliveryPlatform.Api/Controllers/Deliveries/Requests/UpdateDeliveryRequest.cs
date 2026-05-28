using System;

namespace DeliveryPlatform.Api.Controllers.Deliveries.Requests;

public sealed record UpdateDeliveryRequest(
    string Name,
    Guid? PickupNodeId,
    int? Load,
    decimal? WeightKg,
    decimal? VolumeM3,
    int? ProductGroup,
    TimeOnly? WindowStart,
    TimeOnly? WindowEnd,
    int? Priority
);
