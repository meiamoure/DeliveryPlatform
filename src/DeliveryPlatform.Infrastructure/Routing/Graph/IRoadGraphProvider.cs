using System;

namespace DeliveryPlatform.Infrastructure.Routing.Graph;

public interface IRoadGraphProvider
{
    Task<RoadGraph> GetGraphAsync(CancellationToken ct);
}
