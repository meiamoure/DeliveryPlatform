using System;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.BuildManyRoutes;

public sealed record BuildManyRoutesCommand(
    Guid DepotNodeId,
    IReadOnlyCollection<Guid> VehicleIds,
    IReadOnlyCollection<Guid> DeliveryIds,
    DateOnly Date
) : IRequest<BuildManyRoutesResult>;
