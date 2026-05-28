using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryPlatform.Core.Domain.Vehicles.Models;

namespace DeliveryPlatform.Persistence.EFCore.EntityConfigurations;

internal sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Plate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.MaxWeightKg)
    .HasColumnType("decimal(10,2)")
    .IsRequired();

        builder.Property(x => x.MaxVolumeM3)
            .HasColumnType("decimal(10,2)")
            .IsRequired();
        builder.Property(x => x.SpeedKmh).IsRequired();
        builder.Property(x => x.DepotNodeId).IsRequired();

        builder.HasIndex(x => x.Plate).IsUnique();

        // FK to Nodes (no navigation property on Vehicle side)
        builder.HasOne(x => x.DepotNode)
            .WithMany()
            .HasForeignKey(x => x.DepotNodeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
