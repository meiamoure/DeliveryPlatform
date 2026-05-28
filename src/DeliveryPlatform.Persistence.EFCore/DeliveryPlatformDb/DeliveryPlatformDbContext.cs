using DeliveryPlatform.Core.Domain.Deliveries.Models;
using DeliveryPlatform.Core.Domain.Drivers.Models;
using DeliveryPlatform.Core.Domain.Nodes.Models;
using DeliveryPlatform.Core.Domain.Routes.Models;
using DeliveryPlatform.Core.Domain.Vehicles.Models;
using DeliveryPlatform.Core.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;

public class DeliveryPlatformDbContext(DbContextOptions<DeliveryPlatformDbContext> options) : DbContext(options)
{
    public const string DeliveryPlatformDbSchema = "deliveryplatform";

    public const string DeliveryPlatformMigrationHistory = "__DeliveryPlatformMigrationHistory";

    public DbSet<Node> Nodes => Set<Node>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<RouteSegment> RouteSegments => Set<RouteSegment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DeliveryPlatformDbSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryPlatformDbContext).Assembly);
    }
}
