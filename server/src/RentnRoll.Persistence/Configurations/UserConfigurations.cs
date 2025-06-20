using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Persistence.Identity;

namespace RentnRoll.Persistence.Configurations;

internal sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasQueryFilter(u => !u.IsDeleted);

        builder
            .HasIndex(u => u.IsDeleted)
            .HasFilter("IsDeleted = 0");

        builder
            .Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(400)
            .HasColumnType("varchar(400)");

        builder
            .Property(u => u.Country)
            .IsRequired(false)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder
            .Property(u => u.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(u => u.DeletedAt)
            .HasColumnType("datetime")
            .IsRequired(false);

        builder
            .Property(u => u.RefreshToken)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder
            .Property(u => u.RefreshTokenExpiry)
            .HasColumnType("datetime")
            .IsRequired(false);
    }
}