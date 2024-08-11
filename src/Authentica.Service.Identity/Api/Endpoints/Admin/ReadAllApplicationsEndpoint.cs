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
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

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
    public IServiceProvider Services { get; }

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
        var dbContext = Services.GetRequiredService<AppDbContext>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        IList<ClientApplication> apps = await dbContext.ClientApplications.ToListAsync(cancellationToken: cancellationToken);

        IList<ReadApplicationResponse> responses = apps.Select(app => new ClientApplicationMapper().ToResponse(app)).ToList();
        IList<ReadApplicationResponse> redactedResponses = apps.Select(app => new ClientApplicationMapper().ToResponse(app)).ToList();

        ReadAllApplicationsEvent @event = new()
        {
            Payload = redactedResponses
        };

        await eventStore.SaveEventAsync(@event);

        return Ok(responses);
    }
}