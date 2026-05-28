using System;

namespace DeliveryPlatform.Application.Routing.ShortestPath;

public sealed class ShortestPathResult
{
    // минимальная стоимость (время) от source до каждого узла
    public IReadOnlyDictionary<Guid, int> Distances { get; init; } = default!;

    // родители — нужны, чтобы восстановить путь
    public IReadOnlyDictionary<Guid, Guid?> Parents { get; init; } = default!;
}
