using Api.Constants;
using Api.Requests;
using Api.Responses;
using Application.Activities.Users;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;


/// <summary>
/// Endpoint for managing the authenticator used for mfa.
/// Allows enabling or disabling mfa for a user and generates the necessary authenticator key and QR code URI.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class MultiFactorManageAuthenticatorEndpoint : EndpointBaseAsync
                                                    .WithRequest<MultiFactorManageAuthenticatorRequest>
                                                    .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorManageAuthenticatorEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="services"/> is null.</exception>
    public MultiFactorManageAuthenticatorEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the request to manage the user's authenticator for mfa authentication.
    /// Enables or disables mfa for the user, and if enabled, generates the authenticator key and QR code URI.
    /// </summary>
    /// <param name="request">The request containing the information on whether to enable or disable the authenticator.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> containing the formatted authenticator key and QR code URI if 2FA is enabled, or an error message if not.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost($"{Routes.Users.MultiFactorManageAuthenticator}")]
    public override async Task<ActionResult> HandleAsync(MultiFactorManageAuthenticatorRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var totpProvider = Services.GetRequiredService<IMultiFactorTotpProvider>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();
        string formattedKey = string.Empty;
        string uri = string.Empty;

        var user = (await userReadStore.GetUserByEmailAsync(User, cancellationToken)).User;

        if (!user.TwoFactorEnabled)
            return BadRequest("User does not have mfa enabled.");

        user.MultiFactorAuthenticatorEnabled = request.IsEnabled;

        await userManager.UpdateAsync(user);

        if (request.IsEnabled)
        {
            string? unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(unformattedKey))
            {
                 await userManager.ResetAuthenticatorKeyAsync(user);
                 unformattedKey = await totpProvider.GenerateKeyAsync(user);
            } 
            formattedKey = totpProvider.FormatKey(unformattedKey);
            uri = await totpProvider.GenerateQrCodeUriAsync(user);
        }

        MultiFactorManageAuthenticatorResponse response = new()
        {
            AuthenticatorKey = formattedKey,
            QrCodeUri = uri
        };

        MultiFactorManageAuthenticatorActivity activity = new()
        {
            Request = request,
            Response = response
        };

        await activityWriteStore.SaveActivityAsync(activity);

        return Ok(response);
    }
}
