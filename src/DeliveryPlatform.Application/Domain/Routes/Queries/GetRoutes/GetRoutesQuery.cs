using System;
using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.GetRoutes;

public sealed record GetRoutesQuery(
    DateOnly? From = null,
    DateOnly? To = null
) : IRequest<IReadOnlyList<RouteListItemDto>>;
