using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;

public static class DeliveryPlatformDbContextRegistration
{
    public static void RegisterDeliveryPlatformDbContext(this IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DeliveryPlatform");

    services.AddDbContext<DeliveryPlatformDbContext>(options =>
    {
        options.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable(
                    DeliveryPlatformDbContext.DeliveryPlatformMigrationHistory,
                    DeliveryPlatformDbContext.DeliveryPlatformDbSchema);
            });
    });
}
}
