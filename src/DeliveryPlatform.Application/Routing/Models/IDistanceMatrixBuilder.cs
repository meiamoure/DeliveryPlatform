using System;

namespace DeliveryPlatform.Application.Routing.Models;

public interface IDistanceMatrixBuilder
{
    Task<DistanceMatrix> BuildAsync(
        IReadOnlyCollection<Guid> nodeIds,
        CancellationToken ct);
}
