using System;
using MediatR;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Core.Common;
using DeliveryPlatform.Core.Domain.Routes.Data;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.SendRouteToDriver;

public sealed class SendRouteToDriverCommandHandler
    : IRequestHandler<SendRouteToDriverCommand>
{
    private readonly IRouteRepository _routes;
    private readonly IUnitOfWork _uow;

    public SendRouteToDriverCommandHandler(IRouteRepository routes, IUnitOfWork uow)
    {
        _routes = routes;
        _uow = uow;
    }

    public async Task Handle(SendRouteToDriverCommand cmd, CancellationToken ct)
    {
        var route = await _routes.GetRouteByIdAsync(cmd.RouteId, ct)
            ?? throw new KeyNotFoundException($"Route {cmd.RouteId} not found");

        route.Update(new UpdateRouteData(
            Status: route.Status,     // оставляю статус как есть
            DriverId: cmd.DriverId
        ));

        await _routes.UpdateAsync(route, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
