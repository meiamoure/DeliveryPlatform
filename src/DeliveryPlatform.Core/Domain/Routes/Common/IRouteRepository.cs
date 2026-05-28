using DeliveryPlatform.Core.Domain.Routes.Models;

namespace DeliveryPlatform.Core.Domain.Routes.Common;

public interface IRouteRepository
{
    Task<Route?> GetRouteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Route>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Route route, CancellationToken cancellationToken = default);
    Task UpdateAsync(Route route, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Route?> GetActiveByDriverIdAsync(Guid driverId, CancellationToken ct);
    Task<Route?> GetActiveByVehicleIdAsync(Guid vehicleId, CancellationToken ct);
    Task<int> GetLastNumberAsync(CancellationToken ct);
}
