using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Persistence.Configurations;

public class LockerConfigurations : IEntityTypeConfiguration<Locker>
{
    public void Configure(EntityTypeBuilder<Locker> builder)
    {
        builder
            .HasQueryFilter(l => l.IsActive);

        builder.HasKey(l => l.Id);

        builder
            .Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder.OwnsOne(s => s.Address, a =>
        {
            a.WithOwner();
            a.Property(ad => ad.Street).HasMaxLength(200);
            a.Property(ad => ad.City).HasMaxLength(100);
            a.Property(ad => ad.State).HasMaxLength(50);
            a.Property(ad => ad.ZipCode).HasMaxLength(20);
        });

        builder
            .Property(l => l.IsActive)
            .IsRequired();

        builder
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(b => b.UpdatedAt)
            .IsRequired(false);

        builder
            .HasMany(l => l.PricingPolicies)
            .WithMany()
            .UsingEntity(j => j.ToTable("LockerPricingPolicies"));
    }
}