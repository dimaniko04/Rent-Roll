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
            .HasOne(b => b.BusinessGame)
            .WithMany()
            .HasForeignKey(b => b.BusinessGameId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class LockerRentalConfiguration
    : IEntityTypeConfiguration<LockerRental>
{
    public void Configure(EntityTypeBuilder<LockerRental> builder)
    {
        builder.HasKey(lr => lr.RentalId);

        builder
            .HasOne(lr => lr.Rental)
            .WithOne(r => r.LockerRental)
            .HasForeignKey<LockerRental>(lr => lr.RentalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(lr => lr.Locker)
            .WithMany()
            .HasForeignKey(lr => lr.LockerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

public class StoreRentalConfiguration
    : IEntityTypeConfiguration<StoreRental>
{
    public void Configure(EntityTypeBuilder<StoreRental> builder)
    {
        builder.HasKey(sr => sr.RentalId);

        builder
            .HasOne(sr => sr.Rental)
            .WithOne(r => r.StoreRental)
            .HasForeignKey<StoreRental>(sr => sr.RentalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(sr => sr.Store)
            .WithMany()
            .HasForeignKey(sr => sr.StoreId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}