using DeliveryPlatform.Core.Domain.Deliveries.Common;
using DeliveryPlatform.Core.Domain.Deliveries.Models;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using Microsoft.EntityFrameworkCore;

namespace DeliveryPlatform.Infrastructure.Core.Domain.Deliveries.Common;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly DeliveryPlatformDbContext _dbContext;
    public DeliveryRepository(DeliveryPlatformDbContext db) => _dbContext = db;

    public async Task<Delivery?> GetDeliveryByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Deliveries
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<IReadOnlyList<Delivery>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Deliveries
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Delivery>> GetPendingAsync(
    CancellationToken ct)
    {
        return await _dbContext.Deliveries
            .Where(d => d.Status == DeliveryStatus.Pending)
            .ToListAsync(ct);
    }
    public async Task<IReadOnlyList<Delivery>> GetPendingForDateAsync(
        DateOnly date,
        CancellationToken ct)
    {
        return await _dbContext.Deliveries
            .Where(d =>
                d.Status == DeliveryStatus.Pending &&
                (d.ServiceDate == null || d.ServiceDate == date))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Delivery>> GetDeliveriesByIdsAsync(
    IReadOnlyCollection<Guid> ids,
    CancellationToken ct = default)
    {
        if (ids.Count == 0) return Array.Empty<Delivery>();

        return await _dbContext.Deliveries
            .Where(d => ids.Contains(d.Id))
            .ToListAsync(ct);
    }

    public async Task<int> GetNextDeliveryNumber(CancellationToken ct)
    {
        var result = await _dbContext.Database
            .SqlQuery<int>($"SELECT nextval('delivery_number_seq') AS \"Value\"")
            .FirstAsync(ct);

        return result;
    }

    public async Task AddAsync(Delivery delivery, CancellationToken ct = default)
    {
        await _dbContext.Deliveries.AddAsync(delivery, ct);
    }

    public async Task UpdateAsync(Delivery delivery, CancellationToken ct = default)
    {
        _dbContext.Deliveries.Update(delivery);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var delivery = await _dbContext.Deliveries.FindAsync([id], ct);

        if (delivery == null)
            return;

        _dbContext.Deliveries.Remove(delivery);
    }
}
