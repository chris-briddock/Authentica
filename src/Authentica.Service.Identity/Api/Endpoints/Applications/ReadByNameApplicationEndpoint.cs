using Api.Constants;
using Api.Requests;
using Api.Responses;
using Application.Contracts;
using Application.Mappers;
using Ardalis.ApiEndpoints;
using Application.Activities;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Applications;

/// <summary>
/// Exposes an endpoint where users can read their application by name.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadByNameApplicationEndpoint : EndpointBaseAsync
                                                    .WithRequest<ReadApplicationByNameRequest>
                                                    .WithActionResult
{
    /// <summary>
    /// Provides access to application services.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ReadByNameApplicationEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public ReadByNameApplicationEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Allows a user to read an application by name.
    /// </summary>
    /// <param name="request">The object which encapsulates the request body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the requested application.</returns>
    [HttpGet($"{Routes.Applications.ReadByName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync([FromQuery]ReadApplicationByNameRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var readStoreResult = Services.GetRequiredService<IApplicationReadStore>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var user = await userReadStore.GetUserByEmailAsync(User, cancellationToken);
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        ClientApplication? app = await readStoreResult.GetClientApplicationByNameAndUserIdAsync(request.Name,
                                                                                                user.User!.Id,
                                                                                                cancellationToken);

        if (app is null)
            return BadRequest();

        ReadApplicationResponse response = new ClientApplicationMapper().ToResponse(app);

        ReadApplicationByNameActivity activity = new()
        {
            Request = request,
            Response = response
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(response);
    }
}