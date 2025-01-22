using Application.DTOs;
using Domain.Aggregates.Identity;

namespace Domain.Contracts.Stores;

/// <summary>
/// Defines a contract for read operations on application-related data.
/// </summary>
public interface IApplicationReadStore
{
    /// <summary>
    /// Checks if an application with the specified name exists.
    /// </summary>
    /// <param name="applicationName">The name of the application to check.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean indicating if the application exists.</returns>
    Task<bool> CheckApplicationExistsByNameAsync(string applicationName, CancellationToken cancellationToken = default);
    /// <summary>
    /// Checks if an application with the specified name exists.
    /// </summary>
    /// <param name="clientId">The client id of the application.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean indicating if the application exists.</returns>
    Task<bool> CheckApplicationExistsByClientIdAsync(string clientId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a client application by its name and associated user ID.
    /// </summary>
    /// <param name="name">The name of the client application to retrieve.</param>
    /// <param name="userId">The user ID to check for association.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The client application if found, otherwise null.</returns>
    Task<ClientApplication?> GetClientApplicationByNameAndUserIdAsync(string name,
                                                                       string userId,
                                                                       CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a client application by its client id and callback uri.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="callbackUri"></param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the client application if found;
    /// otherwise, null.
    /// </returns>
    Task<ApplicationReadDto?> GetClientAppByClientIdAndCallbackUriAsync(string clientId,
                                                                          string callbackUri,
                                                                          CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all client applications associated with a given user ID.
    /// </summary>
    /// <param name="userId">The user ID to check for association.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of client applications associated with the specified user ID.</returns>
    Task<List<ApplicationReadDto>> GetAllClientApplicationsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a client application by client id.
    /// </summary>
    /// <param name="clientId">The client id for the application</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A client application with the specified client id, which is associated to a user.</returns>
    Task<ApplicationReadDto> GetClientApplicationByClientIdAsync(string clientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all applications.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of client applications.</returns>
    Task<List<ApplicationReadDto>> GetAllApplicationsAsync(CancellationToken cancellationToken = default);
}

