using System;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.BuildRoute;

public sealed record BuildRouteCommand(
    Guid DepotNodeId,
    Guid VehicleId,
    IReadOnlyCollection<Guid> DeliveryIds,
    DateOnly Date
) : IRequest<Guid>;
