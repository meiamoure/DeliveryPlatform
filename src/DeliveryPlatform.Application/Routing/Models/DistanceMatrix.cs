using System;

namespace DeliveryPlatform.Application.Routing.Models;

public sealed class DistanceMatrix
{
    private readonly Dictionary<(Guid From, Guid To), DistanceMatrixEntry> _data;

    public DistanceMatrix(
        Dictionary<(Guid From, Guid To), DistanceMatrixEntry> data)
    {
        _data = data;
    }

    public DistanceMatrixEntry Get(Guid from, Guid to)
    {
        if (!_data.TryGetValue((from, to), out var entry))
            throw new KeyNotFoundException(
                $"Distance from {from} to {to} not found");

        return entry;
    }

    public bool TryGet(Guid from, Guid to, out DistanceMatrixEntry? entry)
    => _data.TryGetValue((from, to), out entry);

    public IReadOnlyCollection<Guid> Nodes =>
        _data.Keys
            .SelectMany(k => new[] { k.From, k.To })
            .Distinct()
            .ToList();
}
