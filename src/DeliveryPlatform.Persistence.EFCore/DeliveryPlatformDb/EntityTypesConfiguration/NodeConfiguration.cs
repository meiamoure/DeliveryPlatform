using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryPlatform.Core.Domain.Nodes.Models;

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.EntityTypesConfiguration;

internal sealed class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("Nodes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Lat).IsRequired();
        builder.Property(x => x.Lng).IsRequired();
        builder.Property(x => x.Type).HasConversion<int>().IsRequired();

        builder.HasIndex(x => x.Name);
    }
}
