using Api.Requests;
using Application.Contracts;
using Application.DTOs;
using Application.Factories;
using Application.Results;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Stores;

/// <summary>
/// Handles write operations related to creating new client applications
/// and updating existing client applications.
/// </summary>
public class ApplicationWriteStore : StoreBase, IApplicationWriteStore
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationWriteStore"/> class.
    /// </summary>
    /// <param name="services">The service provider to retrieve required services for the write store operations.</param>
    /// <remarks>
    /// This constructor initializes the <see cref="ApplicationWriteStore"/> instance by calling the base constructor with the provided service provider.
    /// </remarks>
    public ApplicationWriteStore(IServiceProvider services) : base(services)
    {
    }
    /// <summary>
    /// Adds a new client application to the database and associates it with a user.
    /// </summary>
    /// <param name="dto">A data transfer object containing the information for the client application and the user context.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>An <see cref="ApplicationStoreResult"/> representing the outcome of the operation. It includes the newly created user if successful.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Retrieves the user's email from the claims principal contained in the <paramref name="dto"/>.
    /// 2. Finds the user by their email using the <see cref="UserManager{User}"/>.
    /// 3. Creates a new <see cref="ClientApplication"/> and <see cref="UserClientApplication"/> with the provided data.
    /// 4. Inserts the new client application and user-client association into the database.
    /// 5. Uses a transaction to ensure that both insert operations are atomic.
    /// 6. Rolls back the transaction if an exception occurs, and returns an error result.
    /// </remarks>
    public async Task<ApplicationStoreResult> CreateClientApplicationAsync(ApplicationDto<CreateApplicationRequest> dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var userReadResult = await UserReadStore.GetUserByEmailAsync(dto.ClaimsPrincipal, cancellationToken);

            if (userReadResult.User is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());

            var application = new ClientApplication
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Request.Name,
                CallbackUri = dto.Request.CallbackUri,
                CreatedBy = userReadResult.User.Email!,
                CreatedOnUtc = DateTime.UtcNow
            };

            var userClientApplication = new UserClientApplication
            {
                UserId = userReadResult.User.Id,
                ApplicationId = application.Id
            };

            DbContext.ClientApplications.Add(application);
            DbContext.UserClientApplications.Add(userClientApplication);
            await DbContext.SaveChangesAsync(cancellationToken);

            return ApplicationStoreResult.Success();
        }
        catch (Exception ex)
        {
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


    /// <summary>
    /// Asynchronously updates an existing client application based on the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the details of the client application to be updated.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> which indicates the success or failure of the operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="dto"/> is <c>null</c>.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the process of updating the client application.</exception>
    public async Task<ApplicationStoreResult> UpdateApplicationAsync(ApplicationDto<UpdateApplicationByNameRequest> dto,
                                                                     CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var userReadResult = await UserReadStore.GetUserByEmailAsync(dto.ClaimsPrincipal, cancellationToken);

            if (userReadResult.User is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());

            var application = await ApplicationReadStore.GetClientApplicationByNameAndUserIdAsync(dto.Request.CurrentName,
                                                                                                  userReadResult.User.Id,
                                                                                                  cancellationToken);

            if (application is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.ApplicationNotFound());

            application.Name = dto.Request.NewName ?? application.Name;
            application.CallbackUri = dto.Request.NewCallbackUri ?? application.CallbackUri;
            application.ModifiedBy = userReadResult.User.Email ?? application.ModifiedBy;
            application.ModifiedOnUtc = DateTime.UtcNow;

            DbContext.ClientApplications.Update(application);
            await DbContext.SaveChangesAsync(cancellationToken);

            return ApplicationStoreResult.Success();
        }
        catch (Exception ex)
        {
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


    /// <summary>
    /// Soft deletes a client application by marking it as deleted in the database.
    /// </summary>
    /// <param name="dto">The data transfer object containing the application name to be deleted and the claims principal of the user performing the action.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApplicationStoreResult"/> which indicates the success or failure of the operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="dto"/> is <c>null</c>.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the process of soft deleting the client application.</exception>
    public async Task<ApplicationStoreResult> SoftDeleteApplicationAsync(ApplicationDto<DeleteApplicationByNameRequest> dto,
                                                                         CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var userReadResult = await UserReadStore.GetUserByEmailAsync(dto.ClaimsPrincipal, cancellationToken);

            // Check if the user exists
            if (userReadResult.User is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());

            // Retrieve the application to be deleted based on the provided name and user ID
            var application = await ApplicationReadStore.GetClientApplicationByNameAndUserIdAsync(dto.Request.Name,
                                                                                                  userReadResult.User.Id,
                                                                                                  cancellationToken);

            // Check if the application exists
            if (application is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.ApplicationNotFound());

            // Mark the application as deleted and set the deletion details
            application.IsDeleted = true;
            application.DeletedOnUtc = DateTime.UtcNow;
            application.DeletedBy = userReadResult.User.Id;

            // Execute the SQL command to update the application
            DbContext.ClientApplications.Update(application);
            await DbContext.SaveChangesAsync(cancellationToken);

            // Return success result
            return ApplicationStoreResult.Success();
        }
        catch (Exception ex)
        {
            // Return failure result in case of an exception
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }


    /// <summary>
    /// Generates a new client secret and updates the client application with the hashed secret.
    /// </summary>
    /// <param name="dto">The ID of the client application to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is <c>null</c> or empty.</exception>
    public async Task<ApplicationStoreResult> UpdateClientSecretAsync(ApplicationDto<CreateApplicationSecretRequest> dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var randomStringProvider = Services.GetRequiredService<IRandomStringProvider>();
            var hasher = Services.GetRequiredService<ISecretHasher>();

            var userReadResult = await UserReadStore.GetUserByEmailAsync(dto.ClaimsPrincipal, cancellationToken);

            // Check if the user exists
            if (userReadResult.User is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.UserNotFound());

            // Retrieve the application to be updated based on the provided name and user ID.
            var application = await ApplicationReadStore.GetClientApplicationByNameAndUserIdAsync(dto.Request.Name,
                                                                                                  userReadResult.User.Id,
                                                                                                  cancellationToken);

            // Check if the application exists
            if (application is null)
                return ApplicationStoreResult.Failed(IdentityErrorFactory.ApplicationNotFound());

            var secret = randomStringProvider.GenerateAlphanumeric();

            var hashedSecret = hasher.Hash(secret);

            // Update the client secret
            application.ClientSecret = hashedSecret;
            DbContext.ClientApplications.Update(application);
            await DbContext.SaveChangesAsync(cancellationToken);

            // Return success result
            return ApplicationStoreResult.Success(secret);
        }
        catch (Exception ex)
        {
            // Return failure result in case of an exception
            return ApplicationStoreResult.Failed(IdentityErrorFactory.ExceptionOccurred(ex));
        }
    }
}