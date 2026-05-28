using System;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using MediatR;


namespace DeliveryPlatform.Application.Domain.Routes.Commands.AcceptRoute;
public sealed class AcceptRouteCommandHandler : IRequestHandler<AcceptRouteCommand>
{
    private readonly IRouteRepository _routes;
    private readonly IUnitOfWork _uow;

    public AcceptRouteCommandHandler(
        IRouteRepository routes,
        IUnitOfWork uow)
    {
        _routes = routes;
        _uow = uow;
    }

    public async Task Handle(AcceptRouteCommand request, CancellationToken ct)
    {
        var route = await _routes.GetRouteByIdAsync(request.RouteId, ct)
            ?? throw new KeyNotFoundException($"Route {request.RouteId} not found");

        route.Accept();

        await _uow.SaveChangesAsync(ct);
    }
}
