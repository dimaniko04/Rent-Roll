
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.PricingPolicies;

namespace RentnRoll.Persistence.Configurations;

internal sealed class PricingPolicyConfigurations
    : IEntityTypeConfiguration<PricingPolicy>
{
    public void Configure(EntityTypeBuilder<PricingPolicy> builder)
    {
        builder
            .HasQueryFilter(p => !p.Business.IsDeleted);

        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder
            .Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("varchar(500)");

        builder
            .Property(p => p.TimeUnit)
            .IsRequired()
            .HasConversion<string>();

        builder
            .Property(p => p.UnitCount)
            .HasDefaultValue(1);

        builder
            .Property(p => p.PricePercent)
            .IsRequired();

        builder
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(p => p.UpdatedAt)
            .IsRequired(false);

        builder
            .HasOne(p => p.Business)
            .WithMany(b => b.Policies)
            .HasForeignKey(p => p.BusinessId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder
            .HasMany(p => p.Items)
            .WithOne(pi => pi.Policy)
            .HasForeignKey(i => i.PolicyId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}

internal class PricingPolicyItemConfigurations
    : IEntityTypeConfiguration<PricingPolicyItem>
{
    public void Configure(
        EntityTypeBuilder<PricingPolicyItem> builder)
    {
        builder
            .HasKey(i => new { i.PolicyId, i.GameId });

        builder
            .Property(i => i.Price)
            .IsRequired();

        builder
            .HasOne(i => i.Game)
            .WithMany()
            .HasForeignKey(i => i.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(i => i.Policy)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}