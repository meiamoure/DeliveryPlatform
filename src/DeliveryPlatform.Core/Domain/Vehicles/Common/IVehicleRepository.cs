using DeliveryPlatform.Core.Domain.Vehicles.Models;

namespace DeliveryPlatform.Core.Domain.Vehicles.Common;

public interface IVehicleRepository
{
    Task<Vehicle?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Vehicle>> GetVehiclesByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct);
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
