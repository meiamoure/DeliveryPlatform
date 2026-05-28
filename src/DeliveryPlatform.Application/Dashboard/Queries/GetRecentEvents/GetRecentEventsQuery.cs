using System;
using MediatR;

namespace DeliveryPlatform.Application.Dashboard.Queries.GetRecentEvents;

public sealed record GetRecentEventsQuery
    : IRequest<IReadOnlyList<RecentEventDto>>;
