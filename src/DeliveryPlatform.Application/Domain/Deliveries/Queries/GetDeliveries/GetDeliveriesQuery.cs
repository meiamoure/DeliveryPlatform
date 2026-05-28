using System;
using DeliveryPlatform.Application.Common;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Deliveries.Queries.GetDeliveries;

public record GetDeliveriesQuery(Guid DeliveryId, int Page, int PageSize) : IRequest<PageResponse<DeliveryDto[]>>;
