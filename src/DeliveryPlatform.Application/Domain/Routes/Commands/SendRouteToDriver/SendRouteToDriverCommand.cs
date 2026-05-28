using System;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.SendRouteToDriver;

public sealed record SendRouteToDriverCommand(Guid RouteId, Guid DriverId) : IRequest;
