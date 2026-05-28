using System;

namespace DeliveryPlatform.Application.Routing.ShortestPath;

public interface IShortestPathService
{
    Task<ShortestPathResult> RunFromAsync(
        Guid sourceNodeId,
        CancellationToken ct);
}
