using System;
using Microsoft.EntityFrameworkCore;
using DeliveryPlatform.Core.Domain.Nodes.Common;
using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using DeliveryPlatform.Core.Domain.Nodes.Models;

namespace DeliveryPlatform.Infrastructure.Core.Domain.Nodes.Common;

public class NodeRepository : INodeRepository
{
    private readonly DeliveryPlatformDbContext _dbContext;

    public NodeRepository(DeliveryPlatformDbContext db) => _dbContext = db;

    public async Task<Node?> GetNodeByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Nodes
            .FirstOrDefaultAsync(n => n.Id == id, ct);
    }

    public async Task<Node?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await _dbContext.Nodes
            .FirstOrDefaultAsync(n => n.Name == name, ct);
    }

    public async Task<IReadOnlyList<Node>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Nodes
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Node>> GetNodesByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
    {
        return await _dbContext.Nodes
            .Where(n => ids.Contains(n.Id))
            .ToListAsync(ct);
    }

    public async Task AddAsync(Node node, CancellationToken ct = default)
    {
        await _dbContext.Nodes.AddAsync(node, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Node node, CancellationToken ct = default)
    {
        _dbContext.Nodes.Update(node);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var node = await _dbContext.Nodes.FindAsync([id], ct);

        if (node == null)
            return;

        _dbContext.Nodes.Remove(node);
        await _dbContext.SaveChangesAsync(ct);
    }
}
