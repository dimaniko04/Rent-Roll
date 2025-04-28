using Microsoft.EntityFrameworkCore;

namespace RentnRoll.Persistence.Context;

public class RentnRollDbContext : DbContext
{
    public RentnRollDbContext(DbContextOptions<RentnRollDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IAssemblyMarker).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}