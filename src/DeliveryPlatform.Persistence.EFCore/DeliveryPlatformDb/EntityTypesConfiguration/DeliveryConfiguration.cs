using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryPlatform.Core.Domain.Deliveries.Models;

namespace DeliveryPlatform.Persistence.EFCore.EntityConfigurations;

internal sealed class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("Deliveries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(100);
        builder.Property(x => x.NodeId).IsRequired();
        builder.Property(x => x.PickupNodeId).IsRequired(false);
        builder.Property(x => x.WeightKg)
    .HasColumnType("decimal(10,2)")
    .IsRequired();

        builder.Property(x => x.VolumeM3)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.ProductGroup)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ServiceDate).HasColumnType("date");
        builder.Property(x => x.WindowStart).HasColumnType("time");
        builder.Property(x => x.WindowEnd).HasColumnType("time");
        builder.Property(x => x.Priority).HasConversion<int>().IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();
        builder.HasIndex(x => new { x.Status, x.ServiceDate });

        builder
            .HasOne(d => d.Node)
            .WithMany()
            .HasForeignKey(d => d.NodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DeliveryPlatform.Core.Domain.Nodes.Models.Node>()
               .WithMany()
               .HasForeignKey(x => x.PickupNodeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
