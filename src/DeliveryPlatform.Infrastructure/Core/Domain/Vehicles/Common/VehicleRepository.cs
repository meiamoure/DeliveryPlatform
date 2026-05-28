using System;
using DeliveryPlatform.Core.Domain.Vehicles.Common;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using DeliveryPlatform.Core.Domain.Vehicles.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryPlatform.Infrastructure.Core.Domain.Vehicles.Common;

public class VehicleRepository : IVehicleRepository
{
    public readonly DeliveryPlatformDbContext _dbContext;
    public VehicleRepository(DeliveryPlatformDbContext db) => _dbContext = db;

    public async Task<Vehicle?> GetVehicleByIdAsync(Guid id, CancellationToken ct = default)
         => await _dbContext.Vehicles.FindAsync(id);

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Vehicles
            .Include(v => v.DepotNode)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Vehicle>> GetVehiclesByIdsAsync(
    IReadOnlyCollection<Guid> ids,
    CancellationToken ct)
    {
        return await _dbContext.Vehicles
            .Where(v => ids.Contains(v.Id))
            .ToListAsync(ct);
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _dbContext.Vehicles.Update(vehicle);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var vehicle = await _dbContext.Vehicles.FindAsync(id);

        if (vehicle == null)
            return;

        _dbContext.Vehicles.Remove(vehicle);
        await _dbContext.SaveChangesAsync();
    }
}
