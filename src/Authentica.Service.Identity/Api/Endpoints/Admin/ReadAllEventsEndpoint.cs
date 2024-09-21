using Api.Constants;
using Api.Responses;
using Application.Contracts;
using Application.Mappers;
using Ardalis.ApiEndpoints;
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
public class ReadAllEventsEndpoint : EndpointBaseAsync
                                     .WithoutRequest
                                     .WithActionResult<IList<EventResponse>>
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableTwoFactorEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public ReadAllEventsEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles reading all events in the system.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
     /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpGet($"{Routes.Admin.ReadAllEvents}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    public override async Task<ActionResult<IList<EventResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var dbContext = Services.GetRequiredService<AppDbContext>();

        var events = await dbContext.Events.ToListAsync(cancellationToken);

        var responses = new ReadAllEventsMapper().ToResponse(events);

        return Ok(responses);
    }
}