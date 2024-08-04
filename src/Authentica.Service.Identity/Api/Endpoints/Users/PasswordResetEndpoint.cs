using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint to allow a user to reset their password.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class PasswordResetEndpoint : EndpointBaseAsync
                                            .WithRequest<PasswordResetRequest>
                                            .WithActionResult
{
    /// <summary>
    /// The application service provier.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PasswordResetEndpoint"/>
    /// </summary>
    public PasswordResetEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Allows a user to reset their password.
    /// </summary>
    /// <param name="request">The object which encapsulates the request.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A new <see cref="ActionResult"/></returns>
    [HttpPost($"{Routes.Users.ResetPassword}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(PasswordResetRequest request,
                                                        CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        PasswordResetEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        var userReadResult = await userReadStore.GetUserByEmailAsync(request.Email);

        var result = await userWriteStore.ResetPasswordAsync(userReadResult.User, request.Token, request.NewPassword);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();

    }
}
