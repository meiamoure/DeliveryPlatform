using System;
using Microsoft.EntityFrameworkCore;
using DeliveryPlatform.Core.Domain.Drivers.Common;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using DeliveryPlatform.Core.Domain.Drivers.Models;


namespace DeliveryPlatform.Infrastructure.Core.Domain.Drivers.Common;

public class DriverRepository : IDriverRepository
{
    private readonly DeliveryPlatformDbContext _dbContext;

    public DriverRepository(DeliveryPlatformDbContext db) => _dbContext = db;

    public async Task<Driver?> GetDriverByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Drivers
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<IReadOnlyList<Driver>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Drivers
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Driver driver, CancellationToken ct = default)
    {
        await _dbContext.Drivers.AddAsync(driver, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Driver driver, CancellationToken ct = default)
    {
        _dbContext.Drivers.Update(driver);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var driver = await _dbContext.Drivers.FindAsync([id], ct);

        if (driver == null)
            return;

        _dbContext.Drivers.Remove(driver);
        await _dbContext.SaveChangesAsync(ct);
    }
}
