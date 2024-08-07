using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// An endpoint which allows confirming the users email address.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ConfirmEmailEndpoint : EndpointBaseAsync
                                           .WithRequest<ConfirmEmailRequest>
                                           .WithActionResult
{
    /// <inheritdoc/>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializses a new instance of <see cref="ConfirmEmailEndpoint"/>
    /// </summary>
    /// <param name="services"> The <see cref="IServiceProvider"/></param>
    public ConfirmEmailEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    /// <summary>
    /// Allows a user to confirm their email address.
    /// </summary>
    /// <param name="request">The object which encapsulates the request.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A new <see cref="ActionResult"/></returns>
    [HttpPost($"{Routes.Users.ConfirmEmail}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(ConfirmEmailRequest request,
                                                         CancellationToken cancellationToken = default)
    {
            var writeStore = Services.GetRequiredService<IUserWriteStore>();
            var readStore = Services.GetRequiredService<IUserReadStore>();
            var eventStore = Services.GetRequiredService<IEventStore>();

            var userResult = await readStore.GetUserByEmailAsync(request.Email);

            // NOTE: This code should have been emailed to the user.

            var result = await writeStore.ConfirmEmailAsync(userResult.User, request.Token);

            ConfirmEmailEvent @event = new()
            {
                Payload = request
            };

            await eventStore.SaveEventAsync(@event);

            if (result.Errors.Any())
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

            return Ok(); 
    }
}
