using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Common.Events;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint which allows a user to redeem mfa recovery codes.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class MultiFactorRecoveryCodeRedeemEndpoint : EndpointBaseAsync
                                                     .WithRequest<MultiFactorRecoveryCodeRedeemRequest>
                                                     .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorRecoveryCodeRedeemEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public MultiFactorRecoveryCodeRedeemEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP POST request to redeem a mfa recovery code.
    /// </summary>
    /// <param name="request">The request containing the user's email address and the recovery code.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// <see cref="ActionResult"/> indicating the result of the recovery code redemption.
    /// Returns <see cref="StatusCodes.Status200OK"/> if the recovery code was successfully redeemed.
    /// Returns <see cref="StatusCodes.Status400BadRequest"/> if the email is not found or the recovery code redemption fails.
    /// </returns>
    [HttpPost($"{Routes.Users.MultiFactorAuthentication.RecoveryCodes}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(MultiFactorRecoveryCodeRedeemRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();
        var mfaWriteStore = Services.GetRequiredService<IUserMultiFactorWriteStore>();
        var passkeysStore = Services.GetRequiredService<IPasskeyCredentialWriteStore>();
        var publisher = Services.GetRequiredService<IPublisher>();

        var user = (await userReadStore.GetUserByEmailAsync(request.Email, cancellationToken)).User;

        var result = await userWriteStore.RedeemMultiFactorRecoveryCodeAsync(user, request.Code);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        // Once recovery code is redeemed all multi factor options are reset.
        await userManager.SetTwoFactorEnabledAsync(user, false);

        await mfaWriteStore.SetEmailAsync(false, user.Id);
        await mfaWriteStore.SetPasskeysAsync(false, user.Id);
        await mfaWriteStore.SetAutheticatorAsync(false, user.Id);

        // TODO: DELETE ALL PASSKEYS 

        MultiFactorRecoveryCodesRedeemActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        MfaRecoveryCodeRedeemed @event = new(user.Email!, DateTime.UtcNow);

        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();
    }
}
