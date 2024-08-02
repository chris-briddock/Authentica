using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// An endpoint which allows a user to soft delete their account.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class DeleteAccountEndpoint : EndpointBaseAsync
                                            .WithoutRequest
                                            .WithActionResult
{
    /// <summary>
    /// The application's service provider.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteAccountEndpoint"/>
    /// </summary> 
    public DeleteAccountEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    /// <summary>
    /// Exposes an endpoint that allows the user to soft delete their account.
    /// </summary>
    [HttpDelete($"{Routes.Users.DeleteByEmail}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
            var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
            var eventStore = Services.GetRequiredService<IEventStore>();

            DeleteAccountEvent @event = new()
            {
                Email = User.Identity!.Name!
            };

            await eventStore.SaveEventAsync(@event);
            
            var result = await userWriteStore.SoftDeleteUserAsync(User, cancellationToken);

            if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

            return NoContent();
    }
}
