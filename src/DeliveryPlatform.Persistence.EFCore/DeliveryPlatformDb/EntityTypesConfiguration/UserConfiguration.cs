using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryPlatform.Core.Domain.Users.Models;

namespace DeliveryPlatform.Persistence.EFCore.DeliveryPlatformDb.EntityTypesConfiguration;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Login)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Login)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.DriverId)
            .IsRequired(false);
    }
}
