using Api.Constants;
using Api.Requests;
using Api.Responses;
using Application.Contracts;
using Application.Providers;
using Ardalis.ApiEndpoints;
using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Domain.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Api.Endpoints.OAuth;

/// <summary>
/// Endpoint for issuing tokens.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class TokenEndpoint : EndpointBaseAsync
                                    .WithRequest<TokenRequest>
                                    .WithActionResult
{

    /// <summary>
    /// Provides access to application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="TokenEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public TokenEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles token generation for different grant types.
    /// </summary>
    /// <param name="request">The object which encapsulates the request body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns></returns>
    [HttpPost($"{Routes.OAuth.Token}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(TokenRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        string? email = string.Empty;
        string subject = string.Empty;
        var dbContext = Services.GetRequiredService<AppDbContext>();
        var jwtProvider = Services.GetRequiredService<IJsonWebTokenProvider>();
        var configuration = Services.GetRequiredService<IConfiguration>();
        var hasher = Services.GetRequiredService<ISecretHasher>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        string issuer = configuration.GetRequiredValueOrThrow("Jwt:Issuer");
        string secret = configuration.GetRequiredValueOrThrow("Jwt:Secret");
        string audience = configuration.GetRequiredValueOrThrow("Jwt:Audience");
        int expires = Convert.ToInt16(configuration.GetRequiredValueOrThrow("Jwt:Expires"));
        int refreshExpires = expires + 60;

        ClientApplication? client = await dbContext
                                          .Set<ClientApplication>()
                                          .Where(x => x.ClientId == request.ClientId)
                                          .Where(x => x.RedirectUri == request.RedirectUri)
                                          .FirstAsync(cancellationToken);

        UserClientApplication userClientLink = await dbContext
                                               .Set<UserClientApplication>()
                                               .Where(x => x.ApplicationId == client.Id)
                                               .FirstAsync(cancellationToken);

        var hashResult = hasher.Verify(request.ClientSecret, client.ClientSecret!);

        if (!hashResult)
            return Unauthorized();

        var user = await dbContext
                        .Set<User>()
                        .Where(x => x.Id == userClientLink.UserId)
                        .FirstAsync(cancellationToken);

        if (client is null)
            return Unauthorized();

        if (User.Identity!.IsAuthenticated)
        {
            subject = User.Identity.Name!;
            email = User.Identity.Name!;
        }

        if (user is not null
            && !User.Identity!.IsAuthenticated)
        {
            subject = user.Id;
            email = user.Email;
        }

        var storedState = HttpContext.Session.GetString($"{client.ClientId}_state");

        if (storedState != request.State)
            return Unauthorized();

        if (request.GrantType == TokenConstants.Refresh
            && request.RefreshToken is not null)
        {
            var result = await jwtProvider.TryValidateTokenAsync(request.RefreshToken, secret, issuer, audience);

            if (!result.Success)
                return Unauthorized();
        }

        if (request.GrantType == TokenConstants.AuthorizationCode)
        {
            var storedCode = HttpContext.Session.GetString($"{client.ClientId}_code");
            if (storedCode != request.Code)
                return Unauthorized();
        }

        var tokenResult = await jwtProvider.TryCreateTokenAsync(
            email!, secret, issuer, audience, expires, subject);
        var refreshTokenResult = await jwtProvider.TryCreateRefreshTokenAsync(
            email!, secret, issuer, audience, expires, subject);

        if (tokenResult.Success && refreshTokenResult.Success)
        {
            bool isAccessTokenValid = (await jwtProvider.TryValidateTokenAsync(tokenResult.Token, secret, issuer,
                                                                               audience)).Success;
            bool isRefreshTokenValid = (await jwtProvider.TryValidateTokenAsync(refreshTokenResult.Token, secret,
                                                                                issuer, audience)).Success;
            if (!isAccessTokenValid && !isRefreshTokenValid)
                return Unauthorized();
        }

        TokenResponse response = new()
        {
            AccessToken = tokenResult.Token,
            RefreshToken = refreshTokenResult.Token,
            TokenType = "Bearer",
            Expires = expires.ToString()
        };

        TokenEvent @event = new()
        {
            Request = new TokenRequest()
            {
                GrantType = request.GrantType,
                ClientId = request.ClientId,
                ClientSecret = request.ClientSecret,
                Code = request.Code,
                RedirectUri = request.RedirectUri,
                State = request.State
            },
            Response = new TokenResponse()
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                TokenType = "Bearer",
                Expires = expires.ToString()
            }
        };

        await eventStore.SaveEventAsync(@event);

        return Ok(response);
    }
}