using Microsoft.EntityFrameworkCore;

using RentnRoll.Core.Entities;

namespace RentnRoll.Persistence.Context;

public class RentnRollDbContext : DbContext
{
    public RentnRollDbContext(DbContextOptions<RentnRollDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IAssemblyMarker).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}