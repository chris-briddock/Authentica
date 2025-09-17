using Api.Constants;
using Application.Constants;
using Ardalis.ApiEndpoints;
using Common.Constants;
using Domain.Aggregates.Identity;
using Domain.Contracts.Providers;
using Domain.Contracts.Stores;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for managing passkey-based login for MFA.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class MultiFactorPasskeyAssertionOptionsEndpoint : EndpointBaseAsync
                                                         .WithRequest<MultiFactorPasskeyLoginRequest>
                                                         .WithActionResult
{
    private IServiceProvider Services { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorPasskeyAssertionOptionsEndpoint"/> class.
    /// </summary>
    /// <param name="services">The <see cref="IServiceProvider"/> to be used for retrieving services.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MultiFactorPasskeyAssertionOptionsEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the passkey-based login request.
    /// </summary>
    /// <param name="request">The <see cref="MultiFactorPasskeyLoginRequest"/> containing the email address of the user attempting to log in.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> containing the authentication challenge for WebAuthn.</returns>
    [AllowAnonymous]
    [HttpPost($"{Routes.Users.Passkeys.PasskeysAssertionOptions}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(MultiFactorPasskeyLoginRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        IUserReadStore userReadStore = Services.GetRequiredService<IUserReadStore>();
        IPasskeyTokenProvider<User> tokenProvider = Services.GetRequiredService<IPasskeyTokenProvider<User>>();

        IFeatureManager featureManager = Services.GetRequiredService<IFeatureManager>();

        if (!await featureManager.IsEnabledAsync(FeatureFlagConstants.Passkeys, cancellationToken))
        {
            return NotFound();
        }

        var user =  (await userReadStore.GetUserByEmailAsync(request.Email, cancellationToken)).User;

        var options = await tokenProvider.CreateAssertionOptionsAsync(user, cancellationToken);

        HttpContext.Session.SetString(Fido2Constants.AssertionOptions, options);

        return Ok(options);
    }
}
