using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Shared.Interceptors;

public static class Extensions
{
    /// <summary>
    /// Determines if any owned entities related to the given entity have been added or modified.
    /// </summary>
    /// <param name="entry">The Entity Framework Core <see cref="EntityEntry"/> representing the tracked entity.</param>
    /// <returns>
    /// Returns <c>true</c> if any owned entities have been added or modified; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method is useful for detecting changes in owned entity relationships within EF Core.
    /// Owned entities do not have independent primary keys and are managed as part of their parent entity.
    /// </remarks>
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        // Retreive all navigation references of the entity
        return entry.References.Any(e =>
            // Ensure the reference has a valid target entity (that it actually points to another entity i.e., not null)
            e.TargetEntry != null

            // Checks if the referenced entity is owned (i.e., it belongs to the parent entity and does not have its own identity in the database).
            && e.TargetEntry.Metadata.IsOwned()

            // Check if the owned entity was added or modified
            && (e.TargetEntry.State == EntityState.Added || e.TargetEntry.State == EntityState.Modified) 
        );
    }

}
