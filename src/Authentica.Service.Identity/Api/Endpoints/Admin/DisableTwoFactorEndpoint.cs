using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for reading all applications and returning their responses.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class DisableTwoFactorEndpoint : EndpointBaseAsync
                                        .WithRequest<DisableTwoFactorRequest>
                                        .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableTwoFactorEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public DisableTwoFactorEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles disabling two factor for a given user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
     /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Admin.DisableTwoFactor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    public override async Task<ActionResult> HandleAsync(DisableTwoFactorRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var eventStore = Services.GetRequiredService<IEventStore>();

       var user = await userManager.FindByEmailAsync(request.Email);

       if (user is null)
        return BadRequest();

       await userManager.SetTwoFactorEnabledAsync(user, false);

        DisableTwoFactorEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        return Ok();
    }
}