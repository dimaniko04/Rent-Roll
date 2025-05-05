using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using RentnRoll.Domain.Common;

namespace RentnRoll.Persistence.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        var entries = context
            .ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var softDeletable in entries)
        {
            var entity = softDeletable.Entity;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            softDeletable.State = EntityState.Modified;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}