using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using DeliveryPlatform.Core.Domain.Users.Models;
using DeliveryPlatform.Core.Domain.Users.Common;
using Microsoft.EntityFrameworkCore;

namespace DeliveryPlatform.Infrastructure.Core.Domain.Users.Common;
public sealed class UserRepository : IUserRepository
{
    private readonly DeliveryPlatformDbContext _dbContext;

    public UserRepository(DeliveryPlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken ct)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Login == login, ct);
    }

    public async Task<bool> ExistsByLoginAsync(string login, CancellationToken ct)
    {
        return await _dbContext.Users
            .AnyAsync(x => x.Login == login, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _dbContext.Users.AddAsync(user, ct);
    }
}
