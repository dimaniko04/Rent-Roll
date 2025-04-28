using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RentnRoll.Persistence.Identity;

namespace RentnRoll.Persistence.Context;

public class RentnRollDbContext : IdentityDbContext<User>
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