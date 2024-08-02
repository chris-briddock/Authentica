using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint which generates two factor recovery codes.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class TwoFactorRecoveryCodesEndpoint : EndpointBaseAsync
                                              .WithoutRequest
                                              .WithActionResult
{
    /// <summary>
    /// Gets the service provider.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorRecoveryCodesEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public TwoFactorRecoveryCodesEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP GET request to generate new two-factor recovery codes.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>An <see cref="ActionResult"/> containing the newly generated recovery codes or an error status.</returns>
    [HttpGet($"{Routes.Users.TwoFactorRecoveryCodes}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        TwoFactorRecoveryCodesEvent @event = new()
        {
            Email = User.Identity!.Name!
        };

        await eventStore.SaveEventAsync(@event);

        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        var codes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(userReadResult.User, 10);
        return Ok(codes);
    }
}
