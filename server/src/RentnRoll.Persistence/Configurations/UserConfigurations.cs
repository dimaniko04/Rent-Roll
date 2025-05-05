using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Persistence.Identity;

namespace RentnRoll.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasQueryFilter(u => !u.IsDeleted);

        builder
            .HasIndex(u => u.IsDeleted)
            .HasFilter("IsDeleted = 0");

        builder
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder
            .Property(u => u.Country)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder
            .Property(u => u.BirthDate)
            .IsRequired()
            .HasColumnType("date");

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
    }
}