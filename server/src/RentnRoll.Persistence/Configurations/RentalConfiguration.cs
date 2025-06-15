using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Rentals;
using RentnRoll.Domain.Entities.Rentals.Enums;
using RentnRoll.Persistence.Identity;

namespace RentnRoll.Persistence.Configurations;

public class RentalConfiguration
    : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.HasKey(r => r.Id);

        builder
            .Property(b => b.UserId)
            .IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(c => c.Status)
            .IsRequired()
            .HasDefaultValue(RentalStatus.Expectation)
            .HasConversion<string>();

        builder
            .Property(r => r.StartDate)
            .IsRequired()
            .HasColumnType("datetime");

        builder
            .Property(r => r.EndDate)
            .IsRequired()
            .HasColumnType("datetime");

        builder
            .Property(r => r.TotalPrice)
            .IsRequired();

        builder
            .Property(r => r.GameName)
            .IsRequired()
            .HasMaxLength(300)
            .HasColumnType("varchar(300)");

        builder
            .Property(r => r.Address)
            .IsRequired()
            .HasMaxLength(400);

        builder
            .Property(r => r.LocationName)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(r => r.IotDeviceId)
            .IsRequired()
            .HasMaxLength(100);
    }
}