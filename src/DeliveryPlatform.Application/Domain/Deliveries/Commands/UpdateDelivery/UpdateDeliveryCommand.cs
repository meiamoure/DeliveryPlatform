using System;
using DeliveryPlatform.Core.Common;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.UpdateDelivery;

public sealed record UpdateDeliveryCommand(
    Guid Id,
    string? Name,
    Guid? PickupNodeId,
    decimal? WeightKg,
    decimal? VolumeM3,
    ProductGroup? ProductGroup,
    TimeOnly? WindowStart,
    TimeOnly? WindowEnd,
    DeliveryPriority? Priority
) : IRequest<Unit>;
