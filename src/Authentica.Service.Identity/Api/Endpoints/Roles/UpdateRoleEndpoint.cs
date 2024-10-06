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
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Roles;

/// <summary>
/// Represents an endpoint for updating an existing group.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class UpdateRoleEndpoint : EndpointBaseAsync
                                          .WithRequest<UpdateRoleRequest>
                                          .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRoleEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    public UpdateRoleEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the request to update an existing group.
    /// </summary>
    /// <param name="request">The request containing the updated group information.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An action result indicating the outcome of the update operation.</returns>
    /// <remarks>
    /// This method updates the name of an existing role (group) based on the provided request.
    /// If the update is successful, it returns a 204 No Content response.
    /// If the update fails, it returns a 500 Internal Server Error response.
    /// </remarks>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut($"{Routes.Roles.Update}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(UpdateRoleRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        var currentRole = await roleManager.Roles
                                .Where(x => x.Name == request.Name)
                                .FirstAsync(cancellationToken);
        
        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        currentRole.Name = request.Name;
        currentRole.EntityModificationStatus.ModifiedBy = userReadResult.User.Id; 
        currentRole.EntityModificationStatus.ModifiedOnUtc = DateTime.UtcNow;
        var result = await roleManager.UpdateAsync(currentRole);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError);

        UpdateRoleActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        return NoContent();
    }
}
