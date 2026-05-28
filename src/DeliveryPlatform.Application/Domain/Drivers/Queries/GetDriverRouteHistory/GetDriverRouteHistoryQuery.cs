using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;

namespace DeliveryPlatform.Application.Domain.Drivers.Queries.GetDriverRouteHistory;

public sealed record GetDriverRouteHistoryQuery(Guid DriverId)
    : IRequest<List<DriverRouteHistoryDto>>;
