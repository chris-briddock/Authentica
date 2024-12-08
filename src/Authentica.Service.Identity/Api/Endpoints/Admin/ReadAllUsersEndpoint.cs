using Api.Constants;
using Api.Responses;
using Application.Activities;
using Application.Contracts;
using Application.Mappers;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for reading all users.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class ReadAllUsersEndpoint : EndpointBaseAsync
                                           .WithoutRequest
                                           .WithActionResult<IList<GetUserResponse>>
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ReadAllUsersEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public ReadAllUsersEndpoint(IServiceProvider services)
    {
        Services = services;
    }
    /// <summary>
    /// Reads all users in the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the users being returned</returns>
    [HttpGet($"{Routes.Admin.ReadAllUsers}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult<IList<GetUserResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var readStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        IList<User> query = await readStore.GetAllUsersAsync();

        var response = new GetAllUsersMapper().ToResponse(query);

        ReadAllUsersActivity activity = new()
        {
            Email = User.Identity?.Name ?? "Unknown"
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(response);
    }
}
