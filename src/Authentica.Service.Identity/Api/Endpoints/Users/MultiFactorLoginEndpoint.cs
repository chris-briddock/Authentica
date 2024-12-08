using Api.Constants;
using Api.Requests;
using Application.Activities;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for handling mfa login requests.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class MultiFactorLoginEndpoint : EndpointBaseAsync
                                               .WithRequest<MultiFactorLoginRequest>
                                               .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorLoginEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public MultiFactorLoginEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the mfa login request.
    /// </summary>
    /// <param name="request">The mfa login request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Users.MultiFactorLogin}")]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(MultiFactorLoginRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var signInManager = Services.GetRequiredService<SignInManager<User>>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();
        SignInResult result;

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user is null)
            return BadRequest();

        bool isMfaEnabled = await userManager.GetTwoFactorEnabledAsync(user);

        if (!isMfaEnabled)
            return Unauthorized("User does not have mfa enabled.");

        if (request.UseAuthenticator)
            result = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultAuthenticatorProvider, request.Token, true, true);
        else
            result = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider, request.Token, true, true);

        MultiFactorLoginActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        if (!result.Succeeded)
            return Unauthorized();

        return Ok();
    }
}
