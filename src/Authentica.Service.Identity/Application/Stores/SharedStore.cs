using Application.Contracts;
using Application.Factories;
using Application.Results;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

public class SharedStore : StoreBase, ISharedStore
{
    public SharedStore(IServiceProvider services) : base(services)
    {
    }

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
