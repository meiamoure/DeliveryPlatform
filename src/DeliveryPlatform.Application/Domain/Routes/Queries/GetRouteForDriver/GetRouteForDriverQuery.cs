using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

namespace DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteForDriver;

public sealed record GetRouteForDriverQuery(Guid DriverId) : IRequest<DriverRouteDto?>;
