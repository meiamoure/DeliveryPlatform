using System;
using MediatR;
using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveryById;

public sealed record GetDeliveryByIdQuery(Guid Id) : IRequest<Delivery?>;
