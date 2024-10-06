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

namespace Api.Endpoints.Admin.Roles;

/// <summary>
/// Endpoint for creating a new role.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class CreateRoleEndpoint : EndpointBaseAsync
                                   .WithRequest<CreateRoleRequest>
                                   .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    /// <exception cref="ArgumentNullException">Thrown if services is null.</exception>
    public CreateRoleEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the creation of a new group.
    /// </summary>
    /// <param name="request">The request containing role creation details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An action result indicating the outcome of the operation.</returns>
    /// <response code="201">Returns when the role is successfully created.</response>
    /// <response code="500">Returns when there's an internal server error during role creation.</response>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost($"{Routes.Admin.Roles.Create}")]
    public override async Task<ActionResult> HandleAsync(CreateRoleRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();

        var user = (await userReadStore.GetUserByEmailAsync(User, cancellationToken)).User;

        Role group = new()
        {
            Name = request.Name,
            EntityCreationStatus = new(DateTime.UtcNow, user.Id),
            EntityDeletionStatus = new(false, null, null),
            EntityModificationStatus = new(DateTime.UtcNow, user.Id)
        };

        var result = await roleManager.CreateAsync(group);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError);

        CreateRoleActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        return StatusCode(StatusCodes.Status201Created);
    }
}