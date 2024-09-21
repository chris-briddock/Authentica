using Api.Constants;
using Api.Requests.Groups;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Groups;

/// <summary>
/// Represents an endpoint for deleting a group (soft delete).
/// </summary>
public sealed class DeleteGroupEndpoint : EndpointBaseAsync
                                          .WithRequest<DeleteGroupRequest>
                                          .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    public IServiceProvider Services { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteGroupEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    public DeleteGroupEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the request to delete (soft delete) an existing group.
    /// </summary>
    /// <param name="request">The request containing the name of the group to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>An action result indicating the outcome of the delete operation.</returns>
    /// <remarks>
    /// This method performs a soft delete on the specified group by setting IsDeleted to true
    /// and recording the user who performed the deletion and the time of deletion.
    /// Possible responses:
    /// - 204 No Content: If the group is successfully soft deleted.
    /// - 400 Bad Request: If the specified group is not found.
    /// - 500 Internal Server Error: If there's an error during the update process.
    /// </remarks>
    [HttpDelete($"{Routes.Groups.Delete}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(DeleteGroupRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();

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
        
        return NoContent();
    }
}