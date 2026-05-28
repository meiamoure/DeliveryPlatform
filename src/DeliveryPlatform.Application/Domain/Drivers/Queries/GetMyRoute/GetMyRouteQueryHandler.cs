using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Application.Domain.Routes.Queries.GetRouteDetails;
using DeliveryPlatform.Application.Domain.Routes.Queries.Dtos;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Drivers.Queries.GetMyRoute;

public sealed class GetMyRouteQueryHandler 
    : IRequestHandler<GetMyRouteQuery, RouteDetailsDto?>
{
    private readonly IRouteRepository _routes;
    private readonly IMediator _mediator;

    public GetMyRouteQueryHandler(
        IRouteRepository routes,
        IMediator mediator)
    {
        _routes = routes;
        _mediator = mediator;
    }

    public async Task<RouteDetailsDto?> Handle(
        GetMyRouteQuery request,
        CancellationToken ct)
    {
        var route = await _routes.GetActiveByDriverIdAsync(request.DriverId, ct);

        if (route == null)
            return null;

        return await _mediator.Send(
            new GetRouteDetailsQuery(route.Id),
            ct
        );
    }
}
