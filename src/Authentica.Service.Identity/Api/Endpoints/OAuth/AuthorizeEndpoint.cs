using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Application.DTOs;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.OAuth;

/// <summary>
/// Endpoint for handling OAuth authorization requests.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class AuthorizeEndpoint : EndpointBaseAsync
                                        .WithRequest<AuthorizeRequest>
                                        .WithActionResult
{
    /// <summary>
    /// Provides access to application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public AuthorizeEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the OAuth authorization request.
    /// </summary>
    /// <param name="request">The object which encapsulates the request body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the authorization process.</returns>
    [HttpGet($"{Routes.OAuth.Authorize}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public override async Task<ActionResult> HandleAsync(AuthorizeRequest request,
                                                         CancellationToken cancellationToken = default)
    {

        var randomStringProvider = Services.GetRequiredService<IRandomStringProvider>();
        var readStore = Services.GetRequiredService<IApplicationReadStore>();

        ApplicationDto<AuthorizeRequest> dto = new()
        {
            Request = request,
            ClaimsPrincipal = User
        };

        ClientApplication? client = await readStore.GetClientApplicationByClientIdAndCallbackUri(dto, cancellationToken);

        if (client is null)
            return Unauthorized();

        if (request.ResponseType == TokenConstants.AuthorizationCode)
        {
            var state = randomStringProvider.GenerateAlphanumeric(64);
            HttpContext.Session.SetString($"{request.ClientId}_state", state);
            var code = randomStringProvider.GenerateAlphanumeric(64);
            HttpContext.Session.SetString($"{request.ClientId}_code", code);
            await HttpContext.Session.CommitAsync(cancellationToken);

            var redirectUri = $"{request.CallbackUri}/?code={code}&state={state}";
            return Redirect(redirectUri);
        }

        return Unauthorized();

    }
}