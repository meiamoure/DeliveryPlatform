using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace DeliveryPlatform.Application.Domain.Routes.Commands.StartRoute;

public sealed record StartRouteCommand(Guid RouteId) : IRequest;
