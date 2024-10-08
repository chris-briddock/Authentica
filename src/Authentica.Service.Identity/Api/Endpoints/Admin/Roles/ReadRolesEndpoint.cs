using Api.Constants;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Application.Activities;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Admin.Roles;

/// <summary>
/// Represents an endpoint for retrieving all groups (roles).
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadRolesEndpoint : EndpointBaseAsync
                                        .WithoutRequest
                                        .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadRolesEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    public ReadRolesEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the request to retrieve all groups.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An action result containing the list of all roles (groups).</returns>
    /// <remarks>
    /// This method retrieves all roles from the role manager and returns them as an OK result.
    /// </remarks>
    [HttpGet($"{Routes.Admin.Roles.Read}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        var roles = await roleManager.Roles.ToListAsync(cancellationToken);

        ReadRolesActivity activity = new()
        {
            Email = User.Identity?.Name ?? "Unknown"
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(roles);
    }
}