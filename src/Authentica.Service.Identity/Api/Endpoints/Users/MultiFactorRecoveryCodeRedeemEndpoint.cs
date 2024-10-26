using Api.Constants;
using Api.Requests;
using Application.Activities;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
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
    [HttpPost($"{Routes.Users.MultiFactorRedeemRecoveryCodes}")]
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

        var user = (await userReadStore.GetUserByEmailAsync(request.Email)).User;

        var result = await userWriteStore.RedeemMultiFactorRecoveryCodeAsync(user, request.Code);

        if (!result.Succeeded)
            return BadRequest();

        await userManager.SetTwoFactorEnabledAsync(user, false);

        MultiFactorRecoveryCodesRedeemActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        return Ok();
    }
}
