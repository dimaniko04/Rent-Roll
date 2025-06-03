using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RentnRoll.Domain.Entities.Games;
using RentnRoll.Persistence.Identity;

namespace RentnRoll.Persistence.Configurations;

internal sealed class GameConfigurations : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(300)
            .HasColumnType("varchar(300)");

        builder
            .HasIndex(g => g.Name)
            .IsUnique();

        builder
            .Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(2000)
            .HasColumnType("varchar(2000)");

        builder
            .Property(g => g.ThumbnailUrl)
            .IsRequired()
            .HasMaxLength(400)
            .HasColumnType("varchar(400)");

        builder
            .Property(g => g.PublishedAt)
            .IsRequired()
            .HasColumnType("date");

        builder
            .Property(g => g.MinPlayers)
            .IsRequired();

        builder
            .Property(g => g.MaxPlayers)
            .IsRequired();

        builder
            .Property(g => g.Age)
            .IsRequired();

        builder
            .Property(g => g.AveragePlayTime)
            .IsRequired(false);

        builder
            .Property(g => g.ComplexityScore)
            .IsRequired(false);

        builder
            .Property(g => g.IsVerified)
            .HasDefaultValue(false);

        builder
            .Property(g => g.VerifiedByUserId)
            .IsRequired(false);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(g => g.VerifiedByUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(g => g.CreatedByUserId)
            .IsRequired(false);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(g => g.CreatedByUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(g => g.ThumbnailUrl)
            .IsRequired(false)
            .HasMaxLength(400)
            .HasColumnType("varchar(400)");

        builder
            .OwnsMany(g => g.Images, ConfigureGameImages);

        builder
            .HasMany(g => g.Genres)
            .WithMany(g => g.Games)
            .UsingEntity(j => j.ToTable("GameGenre"));

        builder
            .HasMany(g => g.Categories)
            .WithMany(g => g.Games)
            .UsingEntity(j => j.ToTable("GameCategory"));

        builder
            .HasMany(g => g.Mechanics)
            .WithMany(g => g.Games)
            .UsingEntity(j => j.ToTable("GameMechanic"));
    }

    private void ConfigureGameImages(
        OwnedNavigationBuilder<Game, Image> builder)
    {
        builder
            .HasKey(i => i.Url);

        builder
            .Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(400)
            .HasColumnType("varchar(400)");

        builder
            .WithOwner(g => g.Game)
            .HasForeignKey(i => i.GameId);
    }
}