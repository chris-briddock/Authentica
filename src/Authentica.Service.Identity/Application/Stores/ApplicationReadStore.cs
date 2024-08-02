using System.Security.Claims;
using Api.Requests;
using Application.Contracts;
using Application.DTOs;
using Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Stores;

/// <summary>
/// Provides read operations for application-related data.
/// </summary>
public sealed class ApplicationReadStore : StoreBase, IApplicationReadStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationReadStore"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public ApplicationReadStore(IServiceProvider services) : base(services)
    {
    }

    /// <summary>
    /// Checks if an application with the specified name exists.
    /// </summary>
    /// <param name="applicationName">The name of the application to check.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean indicating if the application exists.</returns>
    public async Task<bool> CheckApplicationExistsAsync(string applicationName, CancellationToken cancellationToken = default)
    {
        return await DbContext.ClientApplications.AnyAsync(a => a.Name == applicationName, cancellationToken);
    }

    /// <summary>
    /// Retrieves a client application by its name and associated user ID.
    /// </summary>
    /// <param name="name">The name of the client application to retrieve.</param>
    /// <param name="userId">The user ID to check for association.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The client application if found, otherwise null.</returns>
    public async Task<ClientApplication?> GetClientApplicationByNameAndUserIdAsync(string name, string userId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(userId);

        var clientApplication = await DbContext.ClientApplications
            .Join(
                DbContext.UserClientApplications,
                app => app.Id,
                userApp => userApp.ApplicationId,
                (app, userApp) => new { app, userApp }
            )
            .Where(joined => joined.app.Name == name && joined.userApp.UserId == userId)
            .Select(joined => joined.app)
            .FirstOrDefaultAsync(cancellationToken);

        return clientApplication;
    }

    /// <summary>
    /// Retrieves a client application by details specified in the provided DTO.
    /// </summary>
    /// <param name="dto">The data transfer object containing the token request and claims principal.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the client application if found;
    /// otherwise, null.
    /// </returns>
    public async Task<ClientApplication?> GetClientApplicationByDetailsAsync(ApplicationDTO<TokenRequest> dto, CancellationToken cancellationToken)
    {
        var userClaimsPrincipal = dto.ClaimsPrincipal.FindFirst(ClaimTypes.Email)!;
        var user = await UserManager.FindByEmailAsync(userClaimsPrincipal.Value);

        if (user is null)
            return null;

        var userClientLink = await DbContext.UserClientApplications
            .Where(x => x.UserId == user.Id)
            .Select(x => x.ApplicationId)
            .ToListAsync(cancellationToken);

        if (userClientLink is null || !userClientLink.Any())
            return null;

        var clientApplication = await DbContext.ClientApplications
            .Where(x => userClientLink.Contains(x.Id))
            .Where(x => x.ClientId == dto.Request.ClientId)
            .Where(x => x.RedirectUri == dto.Request.RedirectUri)
            .Where(x => x.ClientSecret == dto.Request.ClientSecret)
            .FirstOrDefaultAsync(cancellationToken);

        return clientApplication;
    }

    /// <summary>
    /// Retrieves a client application by its client ID specified in the provided DTO.
    /// </summary>
    /// <param name="dto">The data transfer object containing the authorization request and claims principal.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the client application if found;
    /// otherwise, null.
    /// </returns>
    public async Task<ClientApplication?> GetClientApplicationByClientIdAndCallbackUri(ApplicationDTO<AuthorizeRequest> dto, CancellationToken cancellationToken)
    {
        return await DbContext.ClientApplications
            .Where(x => x.ClientId == dto.Request.ClientId)
            .Where(x => x.CallbackUri == dto.Request.CallbackUri)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all client applications associated with a given user ID.
    /// </summary>
    /// <param name="userId">The user ID to check for association.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A list of client applications associated with the specified user ID.</returns>
    public async Task<IEnumerable<ClientApplication>> GetAllClientApplicationsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var clientApplications = await DbContext.ClientApplications
            .Join(
                DbContext.UserClientApplications,
                app => app.Id,
                userApp => userApp.ApplicationId,
                (app, userApp) => new { app, userApp }
            )
            .Where(joined => joined.userApp.UserId == userId)
            .Select(joined => joined.app)
            .ToListAsync(cancellationToken);

        return clientApplications;
    }

}