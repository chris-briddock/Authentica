using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Application.Activities;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for resetting a user's password.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class ResetPasswordEndpoint : EndpointBaseAsync
                                     .WithRequest<AdminPasswordResetRequest>
                                     .WithActionResult
{
    /// <summary>
    /// Gets the service provider for resolving dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetPasswordEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for resolving dependencies.</param>
    public ResetPasswordEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Allows an admin to reset a users password.
    /// </summary>
    /// <param name="request">The reset request containing user details.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Admin.ResetPassword}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(AdminPasswordResetRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return BadRequest();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        await userManager.ResetPasswordAsync(user, token, request.Password);

        ResetPasswordAdminActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        return NoContent();

    }
}
