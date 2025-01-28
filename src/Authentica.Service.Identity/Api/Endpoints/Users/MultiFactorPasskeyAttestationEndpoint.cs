using Api.Constants;
using Application.Constants;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts.Providers;
using Domain.Contracts.Stores;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for managing passkey-based login for MFA.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class MultiFactorPasskeyAttestationEndpoint : EndpointBaseAsync
                                                     .WithRequest<MultiFactorPasskeyAttestationRequest>
                                                     .WithActionResult
{
    private IServiceProvider Services { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorPasskeyAttestationEndpoint"/> class.
    /// </summary>
    /// <param name="services">The <see cref="IServiceProvider"/> to be used for retrieving services.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MultiFactorPasskeyAttestationEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the passkey-based login request.
    /// </summary>
    /// <param name="request">The <see cref="MultiFactorPasskeyAttestationRequest"/> containing the email address of the user attempting to log in.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> containing the authentication result for WebAuthn.</returns>
    [AllowAnonymous]
    [HttpPost($"{Routes.Users.Passkeys.PasskeyAttestation}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(MultiFactorPasskeyAttestationRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        IUserReadStore userReadStore = Services.GetRequiredService<IUserReadStore>();
        IPasskeyTokenProvider<User> tokenProvider = Services.GetRequiredService<IPasskeyTokenProvider<User>>();

        var user =  (await userReadStore.GetUserByEmailAsync(request.Email)).User;

        var options = HttpContext.Session.GetString(Fido2Constants.AttestationOptions)!;
        
        await tokenProvider.CreateCredentialAsync(user, options, request.Response, cancellationToken);

        return Ok();
    }
}
