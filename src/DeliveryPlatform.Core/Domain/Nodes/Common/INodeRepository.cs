using DeliveryPlatform.Core.Domain.Nodes.Models;

namespace DeliveryPlatform.Core.Domain.Nodes.Common;

public interface INodeRepository
{
    Task<Node?> GetNodeByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Node?> GetByNameAsync(string name, CancellationToken ct);
    Task<IReadOnlyList<Node>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Node>> GetNodesByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct);
    Task AddAsync(Node node, CancellationToken cancellationToken = default);
    Task UpdateAsync(Node node, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
