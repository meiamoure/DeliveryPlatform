using System;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Deliveries.Commands.DeleteDelivery;

public sealed record DeleteDeliveryCommand(Guid DeliveryId) : IRequest<Unit>;
