using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint that allows a user to logout.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class LogoutEndpoint : EndpointBaseAsync
                                     .WithoutRequest
                                     .WithActionResult
{
    /// <summary>
    /// Provides access to application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="LogoutEndpoint"/>.
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public LogoutEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP POST request to logout the user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the logout operation.</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost($"{Routes.Users.Logout}")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var signInManager = Services.GetRequiredService<SignInManager<User>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        var result = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        LogoutEvent @event = new()
        {
            Email = result.User.Email!
        };

        await eventStore.SaveEventAsync(@event);

        await signInManager.SignOutAsync();
        return NoContent();
    }
}