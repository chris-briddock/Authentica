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
/// Endpoint for handling two-factor authentication login requests.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class TwoFactorLoginEndpoint : EndpointBaseAsync
                                            .WithRequest<TwoFactorLoginRequest>
                                            .WithActionResult
{
    /// <summary>
    /// Gets the service provider for dependency injection.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorLoginEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public TwoFactorLoginEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the two-factor authentication login request.
    /// </summary>
    /// <param name="request">The two-factor login request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Users.TwoFactorLogin}")]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(TwoFactorLoginRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var signInManager = Services.GetRequiredService<SignInManager<User>>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        TwoFactorLoginEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);


         var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user is null)
            return BadRequest();

        bool isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);

        if (!isTwoFactorEnabled)
            return Unauthorized("User does not have two factor enabled.");

        var signInResult = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider, request.Token, true, true);

        if (!signInResult.Succeeded)
            return Unauthorized();
            
        return Ok();
    }
}
