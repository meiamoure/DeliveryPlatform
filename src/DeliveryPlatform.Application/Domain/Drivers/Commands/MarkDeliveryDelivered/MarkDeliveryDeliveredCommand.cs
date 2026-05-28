using MediatR;

namespace DeliveryPlatform.Application.Domain.Drivers.Commands.MarkDeliveryDelivered;

public sealed record MarkDeliveryDeliveredCommand(Guid Id) : IRequest<Unit>;
