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
public class MultiFactorPasskeysAttestationOptionsEndpoint : EndpointBaseAsync
                                                            .WithRequest<MultiFactorPasskeyLoginRequest>
                                                            .WithActionResult
{
    private IServiceProvider Services { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorPasskeysAttestationOptionsEndpoint"/> class.
    /// </summary>
    /// <param name="services">The <see cref="IServiceProvider"/> to be used for retrieving services.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MultiFactorPasskeysAttestationOptionsEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the attestation options request.
    /// </summary>
    /// <param name="request">The <see cref="MultiFactorPasskeyLoginRequest"/> containing the email address of the user attempting to log in.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> containing the authentication challenge for WebAuthn.</returns>
    [AllowAnonymous]
    [HttpPost($"{Routes.Users.Passkeys.PasskeyAttestationOptions}")]
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

        var user =  (await userReadStore.GetUserByEmailAsync(request.Email)).User;

        var options = await tokenProvider.CreateAttestationOptionsAsync(user);

        HttpContext.Session.SetString(Fido2Constants.AttestationOptions, options);

        return Ok(options);
    }
}