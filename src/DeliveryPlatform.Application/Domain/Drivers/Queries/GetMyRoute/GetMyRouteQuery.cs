using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

namespace DeliveryPlatform.Application.Domain.Drivers.Queries.GetMyRoute;
public sealed record GetMyRouteQuery(Guid DriverId) : IRequest<RouteDetailsDto?>;
