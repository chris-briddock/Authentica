using Api.Constants;
using Api.Requests;
using Api.Responses;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;


[Route($"{Routes.BaseRoute.Name}")]
public class TwoFactorManageAuthenticatorEndpoint : EndpointBaseAsync
                                                    .WithRequest<TwoFactorManageAuthenticatorRequest>
                                                    .WithActionResult

{
    public IServiceProvider Services { get; }

    public TwoFactorManageAuthenticatorEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost($"{Routes.Users.TwoFactorManageAuthenticator}")]
    public override async Task<ActionResult> HandleAsync(TwoFactorManageAuthenticatorRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();
        var totpProvider = Services.GetRequiredService<ITwoFactorTotpProvider>();
        string formattedKey = string.Empty;
        string uri = string.Empty;

        var result = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        if (!result.User.TwoFactorEnabled)
            return BadRequest("User does not have two factor enabled.");

        result.User.TwoFactorAuthenticatorEnabled = request.IsEnabled;

        await userManager.UpdateAsync(result.User);

        if (request.IsEnabled)
        {
            string? unformattedKey = await userManager.GetAuthenticatorKeyAsync(result.User);

            if (string.IsNullOrEmpty(unformattedKey))
            {
                 await userManager.ResetAuthenticatorKeyAsync(result.User);
                 unformattedKey = await totpProvider.GenerateKeyAsync(result.User);
            } 
            formattedKey = totpProvider.FormatKey(unformattedKey);
            uri = await totpProvider.GenerateQrCodeUriAsync(result.User);
        }

        TwoFactorManageAuthenticatorResponse response = new()
        {
            AuthenticatorKey = formattedKey,
            QrCodeUri = uri
        };

        // TODO: Log the event.

        return Ok(response);

    }
}
