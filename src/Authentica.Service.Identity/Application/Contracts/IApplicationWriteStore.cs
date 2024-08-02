using Api.Requests;
using Application.DTOs;
using Application.Results;

namespace Application.Contracts;

/// <summary>
/// Defines a contract which handles operations related to creating new client applications
/// and updating existing client applications.
/// </summary>
public interface IApplicationWriteStore
{
    /// <summary>
    /// Asynchronously adds a new client application to the system based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the client application to be created.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> which indicates the success or failure of the operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="dto"/> is <c>null</c>.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the process of adding the client application.</exception>
    Task<ApplicationStoreResult> CreateClientApplicationAsync(ApplicationDTO<CreateApplicationRequest> dto,
                                                              CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing client application in the system based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated details of the client application.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> which indicates the success or failure of the operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="dto"/> is <c>null</c>.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the process of updating the client application.</exception>
    Task<ApplicationStoreResult> UpdateApplicationAsync(ApplicationDTO<UpdateApplicationByNameRequest> dto,
                                                        CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes an existing client application based on the provided application name.
    /// The deletion is marked with the current user and timestamp.
    /// </summary>
    /// <param name="dto">A dto which encapsulates the request and its related objects to this operation.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> which indicates the success or failure of the operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="dto"/> is <c>null</c>.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the process of soft deleting the client application.</exception>
    Task<ApplicationStoreResult> SoftDeleteApplicationAsync(ApplicationDTO<DeleteApplicationByNameRequest> dto,
                                                            CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a new client secret and updates the client application with the hashed secret.
    /// </summary>
    /// <param name="dto">The ID of the client application to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is <c>null</c> or empty.</exception>
    Task<ApplicationStoreResult> UpdateClientSecretAsync(ApplicationDTO<CreateApplicationSecretRequest> dto,
                                                         CancellationToken cancellationToken = default);

}