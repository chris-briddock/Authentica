using Application.Results;
using Domain.Contracts;

namespace Application.Contracts;

/// <summary>
/// Defines a contract for operations that are generic and can be shared.
/// </summary>
public interface ISharedStore
{
    /// <summary>
    /// Asynchronously purges soft-deleted entries from the store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to purge.</typeparam>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the purge operation.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled.</exception>
    public Task<SharedStoreResult> PurgeEntriesAsync<TEntity>(CancellationToken cancellationToken)
        where TEntity : class, IEntityDeletionStatus<string>;
}