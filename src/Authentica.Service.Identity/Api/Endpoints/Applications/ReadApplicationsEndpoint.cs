using Api.Constants;
using Api.Responses;
using Application.Contracts;
using Application.Mappers;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
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
    /// Provides access to application services.
    /// </summary>
    public IServiceProvider Services { get; }

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
        var eventStore = Services.GetRequiredService<IEventStore>();
        var userResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        if (userResult?.User?.Id is null)
            return BadRequest();

        IEnumerable<ClientApplication> apps = await readStoreResult.GetAllClientApplicationsByUserIdAsync(userResult.User.Id, cancellationToken);

        if (apps is null)
            return BadRequest();

        List<ReadApplicationResponse> responses = apps.Select(app => new ClientApplicationMapper().ToReadByNameResponse(app)).ToList();
        List<ReadApplicationResponse> redactedResponses = apps.Select(app => new ClientApplicationMapper().ToReadByNameResponse(app)).ToList();

        ReadApplicationsEvent @event = new()
        {
            Payload = redactedResponses
        };

        await eventStore.SaveEventAsync(@event);

        return Ok(responses);
    }
}