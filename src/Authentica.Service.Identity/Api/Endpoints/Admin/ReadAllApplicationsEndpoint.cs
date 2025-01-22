using System.Runtime.InteropServices;
using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Contracts.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for reading all applications and returning their responses.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class ReadAllApplicationsEndpoint : EndpointBaseAsync
                                           .WithoutRequest
                                           .WithActionResult<IList<ReadApplicationResponse>>
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadAllApplicationsEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public ReadAllApplicationsEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the request to read all applications.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of application responses.</returns>
    [HttpGet($"{Routes.Admin.ReadAllApplications}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    public override async Task<ActionResult<IList<ReadApplicationResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var applicationReadStore = Services.GetRequiredService<IApplicationReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        var apps = await applicationReadStore.GetAllApplicationsAsync(cancellationToken);

        List<ReadApplicationResponse> response = new(apps.Count);

        var appsSpan = CollectionsMarshal.AsSpan(apps);

        for (int i = 0; i < appsSpan.Length; i++)
        {
            var item = appsSpan[i];
            response.Add(new ReadApplicationResponse()
            {
                ClientId = item.ClientId,
                CallbackUri = item.CallbackUri,
                Name = item.Name,
                IsDeleted = item.EntityDeletionStatus.IsDeleted,
                DeletedBy = item.EntityDeletionStatus.DeletedBy,
                DeletedOnUtc = item.EntityDeletionStatus.DeletedOnUtc,
                ModifiedBy = item.EntityModificationStatus.ModifiedBy,
                ModifiedOnUtc = item.EntityModificationStatus.ModifiedOnUtc,
                CreatedBy = item.EntityCreationStatus.CreatedBy,
                CreatedOnUtc = item.EntityCreationStatus.CreatedOnUtc
            });
        }

        ReadAllApplicationsActivity activity = new()
        {
            Payload = response
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(response);
    }
}