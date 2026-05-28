using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.CompleteRoute;
public sealed record CompleteRouteCommand(Guid RouteId) : IRequest;
