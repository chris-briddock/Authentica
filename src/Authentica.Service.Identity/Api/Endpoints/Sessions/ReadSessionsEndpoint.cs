using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Sessions;

/// <summary>
/// Endpoint for reading all sessions associated to a user.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadSessionsEndpoint : EndpointBaseAsync
                                           .WithoutRequest
                                           .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadSessionsEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public ReadSessionsEndpoint(IServiceProvider services)
    {
        Services = services;
    }
    /// <summary>
    /// Handles reading all sessions associated to a user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the sessions being returned.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet($"{Routes.Sessions.Name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var sessionStore = Services.GetRequiredService<ISessionReadStore>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();

        var user = (await userReadStore.GetUserByEmailAsync(User, cancellationToken)).User;

        List<Session> sessions = await sessionStore.GetAsync(user.Id);

        return Ok(sessions);
    }
}
