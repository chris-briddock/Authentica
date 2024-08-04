using Api.Constants;
using Api.Requests;
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
/// Endpoint for managing two-factor authentication for a user.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class TwoFactorManageEndpoint : EndpointBaseAsync
                                              .WithRequest<TwoFactorManageRequest>
                                              .WithActionResult
{
    /// <summary>
    /// Gets the service provider.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoFactorManageEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public TwoFactorManageEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the two-factor authentication management request.
    /// </summary>
    /// <param name="request">The request containing the two-factor authentication settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, containing the action result.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost($"{Routes.Users.TwoFactorManage}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(TwoFactorManageRequest request, CancellationToken cancellationToken = default)
    {
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);
        var eventStore = Services.GetRequiredService<IEventStore>();

        TwoFactorManageEvent @event = new()
        {
            Email = User.Identity!.Name!,
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        if (!userReadResult.Succeeded)
            return BadRequest();

        await userManager.SetTwoFactorEnabledAsync(userReadResult.User, request.IsEnabled);

        return NoContent();
    }
}
