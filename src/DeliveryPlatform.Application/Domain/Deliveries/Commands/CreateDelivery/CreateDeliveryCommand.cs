using MediatR;
using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.CreateDelivery;

public record CreateDeliveryCommand(
    string Name,
    Guid? PickupNodeId,
    decimal WeightKg,
    decimal VolumeM3,
    ProductGroup ProductGroup,
    TimeOnly? WindowStart,
    TimeOnly? WindowEnd,
    DeliveryPriority Priority
) : IRequest<Guid>;
