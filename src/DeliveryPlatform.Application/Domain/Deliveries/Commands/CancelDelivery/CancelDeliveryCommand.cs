using MediatR;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.CancelDelivery;

public sealed record CancelDeliveryCommand(Guid Id) : IRequest<Unit>;
