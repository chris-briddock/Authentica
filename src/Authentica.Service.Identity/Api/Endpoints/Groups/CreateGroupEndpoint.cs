using System.Security.Claims;
using Api.Constants;
using Api.Requests.Groups;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Groups;

/// <summary>
/// Endpoint for creating a new group.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class CreateGroupEndpoint : EndpointBaseAsync
                                   .WithRequest<CreateGroupRequest>
                                   .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    public IServiceProvider Services { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGroupEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    /// <exception cref="ArgumentNullException">Thrown if services is null.</exception>
    public CreateGroupEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the creation of a new group.
    /// </summary>
    /// <param name="request">The request containing group creation details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <response code="201">Returns when the group is successfully created.</response>
    /// <response code="500">Returns when there's an internal server error during group creation.</response>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost($"{Routes.Groups.Create}")]
    public override async Task<ActionResult> HandleAsync(CreateGroupRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();

        Role group = new()
        {
            Name = request.Name
        };

        var result = await roleManager.CreateAsync(group);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return StatusCode(StatusCodes.Status201Created);
    }
}