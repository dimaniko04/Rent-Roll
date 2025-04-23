using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentnRoll.Core.Entities;

namespace RentnRoll.Persistence.Configurations;

public class TestEntityConfiguration : IEntityTypeConfiguration<TestEntity>
{
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.ToTable("TestEntities");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("Id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .HasColumnName("Name");

        builder.Property(e => e.Hidden)
            .IsRequired()
            .HasColumnType("varchar(20)")
            .HasMaxLength(20)
            .HasColumnName("Hidden");
    }
}