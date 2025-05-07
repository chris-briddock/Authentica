using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Domain.Events;
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
public sealed class MultiFactorAuthenticatorLoginEndpoint : EndpointBaseAsync
                                                            .WithRequest<MultiFactorLoginRequest>
                                                            .WithActionResult
{

    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Handles the mfa login with an authenticator app.
    /// </summary>
    /// <param name="request">The mfa login request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Users.MultiFactorAuthentication.LoginAuthenticator}")]
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

        var isAuthenticatorEnabled = await mfaReadStore.IsAuthenticatorEnabledAsync(user.Id, cancellationToken);

        if (!isAuthenticatorEnabled.MultiFactorPasskeysEnabled)
            return Unauthorized("User does not have authenticator enabled.");

        result = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultAuthenticatorProvider, request.Token, true, true);

        MultiFactorAuthenticatorLoginActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        if (!result.Succeeded)
            return Unauthorized();
        
        MfaAuthenticatorCodeVerified @event = new(user.Email!, DateTime.UtcNow);
        
        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();
    }
}
