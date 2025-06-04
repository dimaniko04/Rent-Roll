using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Persistence.Identity;

namespace RentnRoll.Persistence.Configurations;

internal sealed class BusinessConfigurations
    : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(300)
            .HasColumnType("varchar(300)");

        builder
            .HasIndex(b => b.Name)
            .IsUnique();

        builder
            .Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(2000)
            .HasColumnType("varchar(2000)");

        builder
            .Property(b => b.OwnerId)
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(b => b.DeletedAt)
            .IsRequired(false);

        builder
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(b => b.UpdatedAt)
            .IsRequired(false);
    }
}