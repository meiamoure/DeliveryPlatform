using MediatR;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Application.Domain.Drivers.Queries.GetDriverRouteHistory;

public sealed class GetDriverRouteHistoryQueryHandler
    : IRequestHandler<GetDriverRouteHistoryQuery, List<DriverRouteHistoryDto>>
{
    private readonly IRouteRepository _routes;

    public GetDriverRouteHistoryQueryHandler(IRouteRepository routes)
    {
        _routes = routes;
    }

    public async Task<List<DriverRouteHistoryDto>> Handle(
        GetDriverRouteHistoryQuery q,
        CancellationToken ct)
    {
        var routes = await _routes.GetAllAsync(ct);

        return routes
            .Where(r =>
                r.DriverId == q.DriverId &&
                r.Status == RouteStatus.Completed
            )
            .OrderByDescending(r => r.ServiceDate)
            .Select(r => new DriverRouteHistoryDto(
                r.Id,
                r.ServiceDate,
                r.Status.ToString(),
                r.TotalDistanceKm,
                r.TotalTimeMin,
                r.Number,
                r.Code
            ))
            .ToList();
    }
}
