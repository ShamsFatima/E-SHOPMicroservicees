using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor:SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries<Domain.Abstractions.IEntity<object>>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = "system";
                }
                else if (entry.State == EntityState.Modified ||
                         entry.HasChangedOwnedEntries())
                {
                    entry.Entity.LastModified = now;
                    entry.Entity.LastModifiedBy = "system";
                }
            }
        }

    }
    public static class EntityEntryExtensions
    {
        public static bool HasChangedOwnedEntries(this Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            return entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == Microsoft.EntityFrameworkCore.EntityState.Added ||
                 r.TargetEntry.State == Microsoft.EntityFrameworkCore.EntityState.Modified));
        }
    }
}
