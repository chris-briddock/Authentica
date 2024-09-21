using Api.Constants;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Groups;

/// <summary>
/// Represents an endpoint for retrieving all groups (roles).
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadGroupsEndpoint : EndpointBaseAsync
                                        .WithoutRequest
                                        .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    public IServiceProvider Services { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadGroupsEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    public ReadGroupsEndpoint(IServiceProvider services)
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
    [HttpGet($"{Routes.Groups.Read}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();

        var roles = await roleManager.Roles.ToListAsync();

        return Ok(roles);
    }
}