using Application.Results;
using Domain.Contracts;

namespace Application.Contracts;

public interface ISharedStore
{
    /// <summary>
    /// Asynchronously purges soft-deleted entries from the store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to purge. This type must implement <see cref="ISoftDeletableEntity{TKey}"/>.</typeparam>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="UserStoreResult"/> indicating the outcome of the purge operation.</returns>
    /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled.</exception>
    /// <remarks>
    /// This method is used to remove entries that have been marked as soft-deleted from the store. The <typeparamref name="TEntity"/> type must implement 
    /// <see cref="ISoftDeletableEntity{TKey}"/> to support soft deletion. The purge operation can be cancelled by passing a cancellation token.
    /// </remarks>
    public Task<SharedStoreResult> PurgeEntriesAsync<TEntity>(CancellationToken cancellationToken)
        where TEntity : class, ISoftDeletableEntity<string>;
}