using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.AcceptRoute;

public sealed record AcceptRouteCommand(Guid RouteId) : IRequest;
