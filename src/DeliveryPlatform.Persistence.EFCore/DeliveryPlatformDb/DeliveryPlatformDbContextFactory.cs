using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb;

public class DeliveryPlatformDbContextFactory : IDesignTimeDbContextFactory<DeliveryPlatformDbContext>
{
    public DeliveryPlatformDbContext CreateDbContext(string[] args)
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var optionsBuilder = new DbContextOptionsBuilder<DeliveryPlatformDbContext>();
    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DeliveryPlatform"));

    return new DeliveryPlatformDbContext(optionsBuilder.Options);
}
}
