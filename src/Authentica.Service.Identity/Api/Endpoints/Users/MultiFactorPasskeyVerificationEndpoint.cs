using Api.Constants;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for verifying passkey-based login for MFA.
/// </summary>
//[Route($"{Routes.BaseRoute.Name}")]
//public class MultiFactorPasskeyVerificationEndpoint : EndpointBaseAsync
//                                                     .WithoutRequest
//                                                     .WithActionResult
//{
//    private IServiceProvider Services { get; }

//    public MultiFactorPasskeyVerificationEndpoint(IServiceProvider services)
//    {
//        Services = services ?? throw new ArgumentNullException(nameof(services));
//    }

//    /// <summary>
//    /// Handles the passkey-based login request.
//    /// </summary>
//    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
//    /// <returns>An <see cref="ActionResult"/> containing the authentication challenge for WebAuthn.</returns>
//    [AllowAnonymous]
//    [HttpPost($"{Routes.Users.MultiFactorPasskeyVerification}")]
//    [ProducesResponseType(StatusCodes.Status200OK)]
//    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
//    {
//    }
//}