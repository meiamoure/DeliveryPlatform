using Microsoft.EntityFrameworkCore;
using DeliveryPlatform.Core.Domain.Routes.Common;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using DeliveryPlatform.Core.Domain.Routes.Models;
using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Infrastructure.Core.Domain.Routes.Common;

public class RouteRepository : IRouteRepository
{
    public readonly DeliveryPlatformDbContext _dbContext;

    public RouteRepository(DeliveryPlatformDbContext db) => _dbContext = db;
    public async Task<Route?> GetRouteByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Routes
            .Include(r => r.Segments)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<IReadOnlyList<Route>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Routes
            .Include(r => r.Segments)
            .AsNoTracking()
            .OrderByDescending(r => r.ServiceDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Route route, CancellationToken ct = default)
    {
        await _dbContext.Routes.AddAsync(route, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Route route, CancellationToken ct = default)
    {
        _dbContext.Routes.Update(route);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var route = await _dbContext.Routes.FindAsync([id], ct);

        if (route == null)
            return;

        _dbContext.Routes.Remove(route);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<Route?> GetActiveByDriverIdAsync(Guid driverId, CancellationToken ct)
    {
        return await _dbContext.Routes
            .FirstOrDefaultAsync(r =>
                r.DriverId == driverId &&
                (r.Status == RouteStatus.Planned ||
                 r.Status == RouteStatus.Accepted ||
                 r.Status == RouteStatus.InProgress),
                ct);
    }

    public async Task<Route?> GetActiveByVehicleIdAsync(Guid vehicleId, CancellationToken ct)
    {
        return await _dbContext.Routes
            .FirstOrDefaultAsync(r =>
                r.VehicleId == vehicleId &&
                (r.Status == RouteStatus.Planned ||
                 r.Status == RouteStatus.Accepted ||
                 r.Status == RouteStatus.InProgress),
                ct);
    }

    public async Task<int> GetLastNumberAsync(CancellationToken ct)
    {
        return await _dbContext.Routes
            .OrderByDescending(r => r.Number)
            .Select(r => r.Number)
            .FirstOrDefaultAsync(ct);
    }
}
