using Application.Results;
using System.Security.Claims;

namespace Domain.Contracts.Stores;

/// <summary>
/// Defines a contract that handles operations related to creating new client applications,
/// updating existing client applications, soft deletion, and secret management.
/// </summary>
public interface IApplicationWriteStore
{
    /// <summary>
    /// Asynchronously adds a new client application to the system based on the provided details.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal of the user initiating the operation.</param>
    /// <param name="name">The name of the new client application.</param>
    /// <param name="callbackUri">The callback URI for the new client application.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> 
    /// indicating the success or failure of the operation.
    /// </returns>
    Task<ApplicationStoreResult> CreateClientApplicationAsync(ClaimsPrincipal claimsPrincipal,
                                                              string name,
                                                              string callbackUri,
                                                              CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing client application in the system based on the provided details.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal of the user initiating the operation.</param>
    /// <param name="name">The new name of the client application, or <c>null</c> to retain the current name.</param>
    /// <param name="callbackUri">The new callback URI for the client application, or <c>null</c> to retain the current URI.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> 
    /// indicating the success or failure of the operation.
    /// </returns>
    Task<ApplicationStoreResult> UpdateApplicationAsync(ClaimsPrincipal claimsPrincipal,
                                                        string? name,
                                                        string? callbackUri,
                                                        CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes an existing client application based on the provided application name.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal of the user initiating the operation.</param>
    /// <param name="name">The name of the client application to delete.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> 
    /// indicating the success or failure of the operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is <c>null</c> or empty.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the process of soft deleting the client application.</exception>
    Task<ApplicationStoreResult> SoftDeleteApplicationAsync(ClaimsPrincipal claimsPrincipal,
                                                            string name,
                                                            CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a new client secret and updates the client application with the hashed secret.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal of the user initiating the operation.</param>
    /// <param name="applicationName">The name of the client application to update.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> 
    /// indicating the success or failure of the operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="applicationName"/> is <c>null</c> or empty.</exception>
    Task<ApplicationStoreResult> UpdateClientSecretAsync(ClaimsPrincipal claimsPrincipal,
                                                         string applicationName,
                                                         CancellationToken cancellationToken = default);
}
