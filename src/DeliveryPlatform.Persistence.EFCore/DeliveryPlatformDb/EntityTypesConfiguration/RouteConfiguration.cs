using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryPlatform.Core.Domain.Routes.Models;

namespace DeliveryPlatform.Persistence.EFCore.EntityConfigurations;

internal sealed class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("Routes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.VehicleId).IsRequired();
        builder.Property(x => x.DriverId);
        builder.Property(x => x.ServiceDate).HasColumnType("date");
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();
        builder.HasIndex(r => r.Number).IsUnique();
        builder.HasIndex(r => r.Code).IsUnique();

        // FK to Vehicle
        builder.HasOne<DeliveryPlatform.Core.Domain.Vehicles.Models.Vehicle>()
               .WithMany()
               .HasForeignKey(x => x.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);

        // FK to Driver (optional)
        builder.HasOne<DeliveryPlatform.Core.Domain.Drivers.Models.Driver>()
               .WithMany()
               .HasForeignKey(x => x.DriverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(nameof(Route.Segments)).AutoInclude(false);
    }
}
