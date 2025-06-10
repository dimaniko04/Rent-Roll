using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Persistence.Configurations;

public class StoreConfigurations : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasQueryFilter(s => !s.Business.IsDeleted);

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(s => s.Address, a =>
        {
            a.WithOwner();
            a.Property(ad => ad.Street).HasMaxLength(200);
            a.Property(ad => ad.City).HasMaxLength(100);
            a.Property(ad => ad.State).HasMaxLength(50);
            a.Property(ad => ad.ZipCode).HasMaxLength(20);
        });

        builder
            .Property(s => s.PolicyId)
            .IsRequired(false);

        builder
            .HasOne(s => s.Policy)
            .WithMany()
            .HasForeignKey(s => s.PolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.Business)
            .WithMany(b => b.Stores)
            .HasForeignKey(s => s.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired(false);

        builder
            .OwnsMany(s => s.Assets, ConfigureStoreAsset);
    }

    private void ConfigureStoreAsset(
        OwnedNavigationBuilder<Store, StoreAsset> builder)
    {
        builder
            .WithOwner(sa => sa.Store)
            .HasForeignKey(sa => sa.StoreId);

        builder.HasKey(sa => new { sa.StoreId, sa.BusinessGameId });

        builder
            .HasOne(sa => sa.BusinessGame)
            .WithMany()
            .HasForeignKey(sa => sa.BusinessGameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(sa => sa.Quantity)
            .IsRequired()
            .HasDefaultValue(1);
    }
}