using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.Entities.Lockers.Enums;

namespace RentnRoll.Persistence.Configurations;

public class CellConfigurations
    : IEntityTypeConfiguration<Cell>
{
    public void Configure(EntityTypeBuilder<Cell> builder)
    {
        builder.HasQueryFilter(
            c => c.Locker.IsActive);

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
            .HasOne(c => c.Locker)
            .WithMany(l => l.Cells)
            .HasForeignKey(c => c.LockerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}