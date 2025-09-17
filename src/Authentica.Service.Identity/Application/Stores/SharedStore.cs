using Application.Factories;
using Application.Results;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Stores;

/// <summary>
/// Represents a shared store that manages entities.
/// </summary>
public class SharedStore : StoreBase, ISharedStore
{
    private readonly ILogger<SharedStore> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SharedStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve services.</param>
    public SharedStore(IServiceProvider services) : base(services)
    {
        _logger = services.GetRequiredService<ILogger<SharedStore>>();
    }

    /// <inheritdoc />
    public async Task<SharedStoreResult> PurgeEntriesAsync<TEntity>(CancellationToken cancellationToken)
    where TEntity : class, IEntityDeletionStatus<string>
    {
        try
        {
            var sevenYearsAgo = DateTime.UtcNow.AddYears(-7).Date;
            var dbSet = DbContext.Set<TEntity>();

            var toBeDeleted = await dbSet
                                    .Where(u => u.EntityDeletionStatus.DeletedOnUtc < sevenYearsAgo)
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
            _logger.LogError(ex, "Error purging entries of type {EntityType} older than {CutoffDate}", typeof(TEntity).Name, DateTime.UtcNow.AddYears(-7).Date);
            return SharedStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}