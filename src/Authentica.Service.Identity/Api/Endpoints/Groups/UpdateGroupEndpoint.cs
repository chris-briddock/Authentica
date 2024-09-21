using Api.Constants;
using Api.Requests.Groups;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Groups;

/// <summary>
/// Represents an endpoint for updating an existing group.
/// </summary>
public sealed class UpdateGroupEndpoint : EndpointBaseAsync
                                          .WithRequest<UpdateGroupRequest>
                                          .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    public IServiceProvider Services { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateGroupEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    public UpdateGroupEndpoint(IServiceProvider services)
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
    [HttpPut($"{Routes.Groups.Update}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(UpdateGroupRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var roleManager = Services.GetRequiredService<RoleManager<Role>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();

        var currentRole = await roleManager.Roles
                                .Where(x => x.Name == request.Name)
                                .FirstAsync(cancellationToken: cancellationToken);
        
        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        currentRole.Name = request.Name;
        currentRole.ModifiedBy = userReadResult.User.Id; 
        currentRole.ModifiedOnUtc = DateTime.UtcNow;
        var result = await roleManager.UpdateAsync(currentRole);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}
