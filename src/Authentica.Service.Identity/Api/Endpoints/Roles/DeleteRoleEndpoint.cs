using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Application.Activities;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Roles;

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
    public IServiceProvider Services { get; set; }

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
    [HttpDelete($"{Routes.Roles.Delete}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(DeleteRoleRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        Role? role = await roleManager.FindByNameAsync(request.Name);
        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        if (role is null)
            return BadRequest();

        role.IsDeleted = true;
        role.DeletedBy = userReadResult.User.Id;
        role.DeletedOnUtc = DateTime.UtcNow;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError);

        DeleteRoleActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        return NoContent();
    }
}