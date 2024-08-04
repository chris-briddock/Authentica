using Api.Requests;
using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain.Aggregates.Identity;
using Domain.Events;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for updating a user's email.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class UpdateEmailEndpoint : EndpointBaseAsync
                                          .WithRequest<UpdateEmailRequest>
                                          .WithActionResult
{
    /// <summary>
    /// Gets the service provider instance for resolving services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmailEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when the services parameter is null.</exception>
    public UpdateEmailEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP PUT request to update a user's email.
    /// </summary>
    /// <param name="request">The request containing the new address details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    /// <response code="200">The user's address was successfully updated.</response>
    [HttpPut($"{Routes.Users.UpdateEmail}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(UpdateEmailRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        UpdateEmailEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        var userResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        var result = await userWriteStore.UpdateEmailAsync(userResult.User, request.Email, request.Token);

        if (!result.Succeeded)
            return BadRequest();

        return Ok();
    }
}

