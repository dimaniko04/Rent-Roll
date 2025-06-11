using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.Entities.Lockers.Enums;

namespace RentnRoll.Persistence.Configurations;

public class LockerConfigurations : IEntityTypeConfiguration<Locker>
{
    public void Configure(EntityTypeBuilder<Locker> builder)
    {
        builder.HasQueryFilter(l => !l.IsDeleted);

        builder.HasKey(l => l.Id);

        builder
            .Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder
            .ComplexProperty(l => l.Address, a =>
            {
                a.Property(ad => ad.Street)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("varchar(200)");
                a.Property(ad => ad.City)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)");
                a.Property(ad => ad.State)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)");
                a.Property(ad => ad.Country)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)");
                a.Property(ad => ad.ZipCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("varchar(20)");
            });

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

        builder
            .HasMany(l => l.PricingPolicies)
            .WithMany()
            .UsingEntity(j => j.ToTable("LockerPricingPolicies"));

        builder
            .OwnsMany(l => l.Cells, ConfigureCell);
    }

    private void ConfigureCell(
        OwnedNavigationBuilder<Locker, Cell> builder)
    {
        builder
            .HasKey(c => c.Id);

        builder
            .Property(c => c.Status)
            .IsRequired()
            .HasDefaultValue(CellStatus.Empty)
            .HasConversion<string>();

        builder
            .Property(c => c.IotDeviceId)
            .IsRequired(false)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder
            .HasOne(c => c.Business)
            .WithMany()
            .HasForeignKey(c => c.BusinessId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .HasOne(c => c.BusinessGame)
            .WithMany()
            .HasForeignKey(c => c.BusinessGameId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .WithOwner(c => c.Locker)
            .HasForeignKey(c => c.LockerId);
    }
}