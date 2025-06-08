using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Persistence.Configurations;

internal sealed class TagConfigurations
    : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasQueryFilter(t => !t.Business.IsDeleted);

        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder
            .HasIndex(t => new { t.Name, t.BusinessId })
            .IsUnique();

        builder
            .Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("varchar(500)");

        builder
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(t => t.UpdatedAt)
            .IsRequired(false);

        builder
            .HasOne(t => t.Business)
            .WithMany(b => b.Tags)
            .HasForeignKey(t => t.BusinessId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}