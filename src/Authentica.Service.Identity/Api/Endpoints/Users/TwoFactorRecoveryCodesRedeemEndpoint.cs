using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint which allows a user to redeem two factor codes.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class TwoFactorRecoveryCodeRedeemEndpoint : EndpointBaseAsync
                                                   .WithRequest<TwoFactorRecoveryCodeRedeemRequest>
                                                   .WithActionResult
{
    /// <summary>
    /// The application service provider.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorRecoveryCodeRedeemEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public TwoFactorRecoveryCodeRedeemEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP POST request to redeem a two-factor recovery code.
    /// </summary>
    /// <param name="request">The request containing the user's email address and the recovery code.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// <see cref="ActionResult"/> indicating the result of the recovery code redemption.
    /// Returns <see cref="StatusCodes.Status200OK"/> if the recovery code was successfully redeemed.
    /// Returns <see cref="StatusCodes.Status400BadRequest"/> if the email is not found or the recovery code redemption fails.
    /// </returns>
    [HttpPost($"{Routes.Users.TwoFactorRedeemRecoveryCodes}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(TwoFactorRecoveryCodeRedeemRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        TwoFactorRecoveryCodesRedeemEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        var userReadResult = await userReadStore.GetUserByEmailAsync(request.Email);

        var result = await userWriteStore.RedeemTwoFactorRecoveryCodeAsync(userReadResult.User, request.Code);

        if (!result.Succeeded)
            return BadRequest();

        return Ok();
    }
}
