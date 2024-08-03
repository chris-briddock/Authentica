using Application.Contracts;
using Application.Factories;
using Application.Results;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

/// <summary>
/// Represents a shared store that manages entities implementing <see cref="ISoftDeletableEntity{TKey}"/>.
/// </summary>
public class SharedStore : StoreBase, ISharedStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SharedStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve services.</param>
    public SharedStore(IServiceProvider services) : base(services)
    {
    }

    /// <summary>
    /// Purges entries of type <typeparamref name="TEntity"/> that were soft-deleted more than seven years ago.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity, which must implement <see cref="ISoftDeletableEntity{TKey}"/>.</typeparam>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="SharedStoreResult"/> indicating the result of the purge operation.</returns>
    /// <inheritdoc/>
    public async Task<SharedStoreResult> PurgeEntriesAsync<TEntity>(CancellationToken cancellationToken)
    where TEntity : class, ISoftDeletableEntity<string>
    {
        try
        {
            var sevenYearsAgo = DateTime.UtcNow.AddYears(-7).Date;
            var dbSet = DbContext.Set<TEntity>();

            var toBeDeleted = await dbSet
                                    .Where(u => u.DeletedOnUtc < sevenYearsAgo)
                                    .ToListAsync(cancellationToken);

            if (toBeDeleted.Count > 0)
            {
                dbSet.RemoveRange(toBeDeleted);
                await DbContext.SaveChangesAsync(cancellationToken);
            }

            return SharedStoreResult.Success();
        }
        catch (Exception ex)
        {
            return SharedStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}