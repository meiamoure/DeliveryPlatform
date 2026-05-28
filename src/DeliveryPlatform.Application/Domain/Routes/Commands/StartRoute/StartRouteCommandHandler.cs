using System;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Routes.Common;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.StartRoute;
public sealed class StartRouteCommandHandler : IRequestHandler<StartRouteCommand>
{
    private readonly IRouteRepository _routes;
    private readonly IUnitOfWork _uow;

    public StartRouteCommandHandler(
        IRouteRepository routes,
        IUnitOfWork uow)
    {
        _routes = routes;
        _uow = uow;
    }

    public async Task Handle(StartRouteCommand request, CancellationToken ct)
    {
        var route = await _routes.GetRouteByIdAsync(request.RouteId, ct)
            ?? throw new KeyNotFoundException($"Route {request.RouteId} not found");

        route.Start();

        await _uow.SaveChangesAsync(ct);
    }
}
