using DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;
using DeliveryPlatform.Core.Common;

namespace DeliveryPlatform.Infrastructure.Core.Common;

public class UnitOfWork(DeliveryPlatformDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
