using Api.Constants;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for managing passkey-based login for MFA.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class MultiFactorPasskeyLoginEndpoint : EndpointBaseAsync
                                               .WithoutRequest
                                               .WithActionResult
{
    private IServiceProvider Services { get; }

    public MultiFactorPasskeyLoginEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the passkey-based login request.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> containing the authentication challenge for WebAuthn.</returns>
    [AllowAnonymous]
    [HttpPost($"{Routes.Users.MultiFactorPasskeyLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}