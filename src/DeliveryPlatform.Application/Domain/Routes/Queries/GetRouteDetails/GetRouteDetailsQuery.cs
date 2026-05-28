using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteDetails;

public sealed record GetRouteDetailsQuery(Guid RouteId)
    : IRequest<RouteDetailsDto>;
