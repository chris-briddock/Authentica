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

namespace Api.Endpoints.Admin.Roles;

/// <summary>
/// Represents an endpoint for deleting a group (soft delete).
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class DeleteRoleEndpoint : EndpointBaseAsync
                                         .WithRequest<DeleteRoleRequest>
                                         .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteRoleEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    public DeleteRoleEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the request to delete (soft delete) an existing group.
    /// </summary>
    /// <param name="request">The request containing the name of the group to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An action result indicating the outcome of the delete operation.</returns>
    [HttpDelete($"{Routes.Admin.Roles.Delete}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(DeleteRoleRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        Role? role = await roleManager.FindByNameAsync(request.Name);
        var user = (await userReadStore.GetUserByEmailAsync(User, cancellationToken)).User;

        if (role is null)
            return BadRequest();

        role.EntityDeletionStatus = new(true, DateTime.UtcNow, user.Id);
        role.EntityModificationStatus = new(DateTime.UtcNow, user.Id);

        await roleManager.UpdateAsync(role);

        DeleteRoleActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        return NoContent();
    }
}