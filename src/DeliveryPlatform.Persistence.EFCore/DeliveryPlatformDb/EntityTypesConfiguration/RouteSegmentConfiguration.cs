using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryPlatform.Core.Domain.Routes.Models;

namespace DeliveryPlatform.Persistence.EFCore.EntityConfigurations;

internal sealed class RouteSegmentConfiguration : IEntityTypeConfiguration<RouteSegment>
{
    public void Configure(EntityTypeBuilder<RouteSegment> builder)
    {
        builder.ToTable("RouteSegments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.RouteId).IsRequired();
        builder.Property(x => x.Order).IsRequired();
        builder.Property(x => x.FromNodeId).IsRequired();
        builder.Property(x => x.ToNodeId).IsRequired();
        builder.Property(x => x.DistanceKm).IsRequired();
        builder.Property(x => x.TravelTimeMin).IsRequired();
        builder.Property(x => x.DeliveryId);
        builder.HasIndex(x => new { x.RouteId, x.Order });

        builder.HasOne<DeliveryPlatform.Core.Domain.Routes.Models.Route>()
               .WithMany(r => r.Segments)
               .HasForeignKey(x => x.RouteId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<DeliveryPlatform.Core.Domain.Deliveries.Models.Delivery>()
               .WithMany()
               .HasForeignKey(x => x.DeliveryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
