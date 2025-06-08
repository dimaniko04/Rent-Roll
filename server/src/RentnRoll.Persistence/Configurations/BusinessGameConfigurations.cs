using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Persistence.Configurations;

internal sealed class BusinessGameConfigurations
    : IEntityTypeConfiguration<BusinessGame>
{
    public void Configure(EntityTypeBuilder<BusinessGame> builder)
    {
        builder
            .HasQueryFilter(bg => !bg.Business.IsDeleted);

        builder
            .HasKey(bg => bg.Id);

        builder
            .Property(bg => bg.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder
            .Property(bg => bg.BasePrice)
            .IsRequired();

        builder
            .HasOne(bg => bg.Game)
            .WithMany()
            .HasForeignKey(bg => bg.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(bg => bg.Tags)
            .WithMany(t => t.BusinessGames)
            .UsingEntity(j => j.ToTable("BusinessGameTags"));

        builder
            .HasOne(bg => bg.Business)
            .WithMany(b => b.Games)
            .HasForeignKey(bg => bg.BusinessId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}