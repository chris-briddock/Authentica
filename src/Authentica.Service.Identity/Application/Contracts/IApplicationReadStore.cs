using Api.Requests;
using Application.DTOs;
using Domain.Aggregates.Identity;

namespace Application.Contracts;

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
    Task<bool> CheckApplicationExistsAsync(string applicationName, CancellationToken cancellationToken = default);
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
    /// Retrieves a client application based on the details specified in the provided DTO.
    /// </summary>
    /// <param name="dto">The data transfer object containing the token request and claims principal.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the client application if found;
    /// otherwise, null.
    /// </returns>
    Task<ClientApplication?> GetClientApplicationByDetailsAsync(ApplicationDTO<TokenRequest> dto, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves a client application by its client ID specified in the provided DTO.
    /// </summary>
    /// <param name="dto">The data transfer object containing the authorization request and claims principal.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the client application if found;
    /// otherwise, null.
    /// </returns>
    Task<ClientApplication?> GetClientApplicationByClientIdAndCallbackUri(ApplicationDTO<AuthorizeRequest> dto, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all client applications associated with a given user ID.
    /// </summary>
    /// <param name="userId">The user ID to check for association.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of client applications associated with the specified user ID.</returns>
    Task<IEnumerable<ClientApplication>> GetAllClientApplicationsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}

