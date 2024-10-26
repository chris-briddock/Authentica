using Api.Constants;
using Api.Requests;
using Api.Responses;
using Application.Activities;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using ChristopherBriddock.AspNetCore.Extensions;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;
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
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

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
        string email = string.Empty;
        string subject = string.Empty;
        IList<string> roles = [];
        IList<string> scopes = [];

        var dbContext = Services.GetRequiredService<AppDbContext>();
        var jwtProvider = Services.GetRequiredService<IJsonWebTokenProvider>();
        var configuration = Services.GetRequiredService<IConfiguration>();
        var hasher = Services.GetRequiredService<ISecretHasher>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var scopeProvider = Services.GetRequiredService<IScopeProvider>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        string issuer = configuration.GetRequiredValueOrThrow("Jwt:Issuer");
        string secret = configuration.GetRequiredValueOrThrow("Jwt:Secret");
        string audience = configuration.GetRequiredValueOrThrow("Jwt:Audience");
        int expires = Convert.ToInt16(configuration.GetRequiredValueOrThrow("Jwt:Expires"));


        ClientApplication? client = await dbContext
                                          .Set<ClientApplication>()
                                          .Where(x => x.ClientId == request.ClientId)
                                          .FirstAsync(cancellationToken);

        UserClientApplication userClientLink = await dbContext
                                               .Set<UserClientApplication>()
                                               .Where(x => x.ApplicationId == client.Id)
                                               .FirstAsync(cancellationToken);

        var userReadResult = await userReadStore.GetUserByIdAsync(userClientLink.UserId);

        var hashResult = hasher.Verify(request.ClientSecret, client.ClientSecret!);

        if (!hashResult)
            return Unauthorized();

        if (client is null)
            return Unauthorized();

        if (!User.Identity!.IsAuthenticated)
        {
            var userEmail = userReadResult.User.Email!;
            roles = await userReadStore.GetUserRolesAsync(userEmail);
            subject = userEmail;
            email = userEmail;
        }
        else 
        {
            var userEmail = User.Identity.Name!;
            roles = await userReadStore.GetUserRolesAsync(userEmail);
            subject = userEmail;
            email = userEmail;
        }

        if (request.GrantType == TokenConstants.DeviceCode)
        {
            var result = await userManager.VerifyUserTokenAsync(userReadResult.User,
                                                                TokenOptions.DefaultEmailProvider,
                                                                TokenConstants.DeviceCode,
                                                                request.DeviceCode!);

            if (!result)
                return Unauthorized();
        }

        if (request.GrantType == TokenConstants.Refresh
            && request.RefreshToken is not null)
        {
            var result = await jwtProvider.TryValidateTokenAsync(request.RefreshToken, secret, issuer, audience);

            if (!result.Success)
                return Unauthorized();
        }

        if (request.GrantType == TokenConstants.AuthorizationCode)
        {
            var storedState = HttpContext.Session.GetString($"{client.ClientId}_state");
            if (storedState != request.State)
                return Unauthorized();
            var storedCode = HttpContext.Session.GetString($"{client.ClientId}_code");
            if (storedCode != request.Code)
                return Unauthorized();
        }
        if (request.Scopes is not null)
            scopes = scopeProvider.ParseScopes(request.Scopes);

        var tokenResult = await jwtProvider.TryCreateTokenAsync(
            email!, secret, issuer, audience, expires, subject, roles, scopes);
        var refreshTokenResult = await jwtProvider.TryCreateRefreshTokenAsync(
            email!, secret, issuer, audience, expires, subject, roles, scopes);

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

        TokenActivity activity = new()
        {
            Request = request
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(response);
    }
}