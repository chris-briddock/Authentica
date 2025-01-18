using System.Runtime.InteropServices;
using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Contracts.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Applications;

/// <summary>
/// Exposes an endpoint where users can read all their applications.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadApplicationsEndpoint : EndpointBaseAsync
                                               .WithoutRequest
                                               .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ReadByNameApplicationEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public ReadApplicationsEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Allows a user to read all applications they have created.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the requested application.</returns>
    [HttpGet($"{Routes.Applications.ReadAll}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var readStoreResult = Services.GetRequiredService<IApplicationReadStore>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        if (userResult?.User?.Id is null)
            return BadRequest();

        var apps = await readStoreResult.GetAllClientApplicationsByUserIdAsync(userResult.User.Id, cancellationToken);

        List<ReadApplicationResponse> responses = new(apps.Count);

        var appsSpan = CollectionsMarshal.AsSpan(apps);

        for (int i = 0; i < appsSpan.Length; i++)
        {
            var app = appsSpan[i];
            responses.Add(new ReadApplicationResponse()
            {
                ClientId = app.ClientId,
                CallbackUri = app.CallbackUri,
                Name = app.Name,
                IsDeleted = app.EntityDeletionStatus.IsDeleted,
                DeletedOnUtc = app.EntityDeletionStatus.DeletedOnUtc,
                DeletedBy = app.EntityDeletionStatus.DeletedBy,
                CreatedOnUtc = app.EntityCreationStatus.CreatedOnUtc,
                CreatedBy = app.EntityCreationStatus.CreatedBy,
                ModifiedBy = app.EntityModificationStatus.ModifiedBy,
                ModifiedOnUtc = app.EntityModificationStatus.ModifiedOnUtc

            });
        }

        ReadApplicationsActivity activity = new()
        {
            Payload = responses
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(responses);
    }
}