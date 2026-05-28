using System;
using MediatR;

namespace DeliveryPlatform.Application.Dashboard.Queries.GetDashboardStats;

public sealed record GetDashboardStatsQuery(DateOnly Date)
    : IRequest<DashboardStatsDto>;
