using Api.Constants;
using Application.Activities;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint which generates mfa recovery codes.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class MultiFactorRecoveryCodesEndpoint : EndpointBaseAsync
                                                .WithoutRequest
                                                .WithActionResult
{
    /// <summary>
    /// Gets the service provider.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorRecoveryCodesEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public MultiFactorRecoveryCodesEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP GET request to generate new mfa recovery codes.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>An <see cref="ActionResult"/> containing the newly generated recovery codes or an error status.</returns>
    [HttpGet($"{Routes.Users.MultiFactorRecoveryCodes}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();

        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        var codes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(userReadResult.User, 10);

        MultiFactorRecoveryCodesActivity activity = new()
        {
            Email = User.Identity?.Name ?? "Unknown",
        };

        await activityWriteStore.SaveActivityAsync(activity);

        return Ok(codes);
    }
}
