using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Core.Domain.Deliveries.Common;

public interface IDeliveryRepository
{
    Task<Delivery?> GetDeliveryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Delivery>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Delivery>> GetPendingAsync(CancellationToken ct);
    Task<IReadOnlyList<Delivery>> GetPendingForDateAsync(DateOnly date, CancellationToken ct);
    Task<IReadOnlyList<Delivery>> GetDeliveriesByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default);
    Task<int> GetNextDeliveryNumber(CancellationToken ct);
    Task AddAsync(Delivery delivery, CancellationToken cancellationToken = default);
    Task UpdateAsync(Delivery delivery, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
