using System;

namespace DeliveryPlatform.Application.Dashboard.Queries.GetDashboardStats;

public sealed record DashboardStatsDto(
    int PendingDeliveriesCount,
    int VehiclesCount,
    int PlannedRoutesCount
);
