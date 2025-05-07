using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Common.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for handling mfa login requests when a token is sent via email.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class MultiFactorEmailLoginEndpoint : EndpointBaseAsync
                                                    .WithRequest<MultiFactorLoginRequest>
                                                    .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorEmailLoginEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public MultiFactorEmailLoginEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the mfa login request.
    /// </summary>
    /// <param name="request">The mfa login request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Users.MultiFactorAuthentication.LoginEmail}")]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(MultiFactorLoginRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var signInManager = Services.GetRequiredService<SignInManager<User>>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();
        var mfaReadStore = Services.GetRequiredService<IUserMultiFactorReadStore>();
        var publisher = Services.GetRequiredService<IPublisher>();

        SignInResult result;

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user is null)
            return BadRequest();

        bool isMfaEnabled = await userManager.GetTwoFactorEnabledAsync(user);

        if (!isMfaEnabled)
            return Unauthorized("User does not have mfa enabled.");

        var isEmailEnabled = await mfaReadStore.IsEmailEnabledAsync(user.Id, cancellationToken);

        if (!isEmailEnabled.MultiFactorEmailEnabled)
            return Unauthorized("User does not have email enabled.");

        result = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider, request.Token, true, true);

        MultiFactorEmailLoginActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        if (!result.Succeeded)
            return Unauthorized();

        MfaEmailCodeVerified @event = new(user.Email!, DateTime.UtcNow);

        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();
    }
}
