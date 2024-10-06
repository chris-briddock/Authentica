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
/// Endpoint for adding a user to a role.
/// </summary>

[Route($"{Routes.BaseRoute.Name}")]
public sealed class AddToRoleEndpoint : EndpointBaseAsync
                                        .WithRequest<AddToRoleRequest>
                                        .WithActionResult
{
    /// <summary>
    /// Gets or sets the service provider for dependency injection.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddToRoleEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for dependency injection.</param>
    /// <exception cref="ArgumentNullException">Thrown if services is null.</exception>
    public AddToRoleEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    /// <summary>
    /// Handles the HTTP PUT request for adding a user to a role.
    /// </summary>
    /// <param name="request">The request containing the user's email and the role to be added.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// <see cref="ActionResult"/> indicating the result of the operation.
    /// Returns <see cref="OkResult"/> if the user is successfully added to the role.
    /// Returns <see cref="BadRequestResult"/> if the operation fails.
    /// </returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut($"{Routes.Admin.Roles.Add}")]
    public override async Task<ActionResult> HandleAsync(AddToRoleRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return BadRequest();

        var result = await userManager.AddToRoleAsync(user, request.Role);

        AddToRoleActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        if (result.Succeeded)
            return Ok();

        return BadRequest();
    }
}
