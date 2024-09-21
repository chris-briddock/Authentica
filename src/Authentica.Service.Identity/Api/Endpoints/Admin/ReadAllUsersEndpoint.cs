using Api.Constants;
using Api.Responses;
using Application.Contracts;
using Application.Mappers;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

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
    /// Provides access to application services.
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
        var dbContext = Services.GetRequiredService<AppDbContext>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        var query = await dbContext.Users.ToListAsync(cancellationToken);

        var response = new GetAllUsersMapper().ToResponse(query);

        return Ok(response);        
    }
}
