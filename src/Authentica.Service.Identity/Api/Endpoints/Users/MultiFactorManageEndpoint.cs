using Api.Constants;
using Api.Requests;
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
/// Endpoint for managing mfa for a user.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class MultiFactorManageEndpoint : EndpointBaseAsync
                                                .WithRequest<MultiFactorManageRequest>
                                                .WithActionResult
{
    /// <summary>
    /// Gets the service provider.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFactorManageEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public MultiFactorManageEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the mfa authentication management request.
    /// </summary>
    /// <param name="request">The request containing the mfa authentication settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, containing the action result.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost($"{Routes.Users.MultiFactorManage}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(MultiFactorManageRequest request, CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();
        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        if (!userReadResult.Succeeded)
            return BadRequest();

        await userManager.SetTwoFactorEnabledAsync(userReadResult.User, request.IsEnabled);

        MultiFactorManageActivity activity = new()
        {
            Email = User.Identity?.Name ?? "Unknown",
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        return NoContent();
    }
}
