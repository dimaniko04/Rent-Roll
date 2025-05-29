using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Persistence.Configurations;

internal sealed class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(g => g.Id);

        builder
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("varchar(200)");

        builder
            .HasIndex(g => g.Name)
            .IsUnique();
    }
}