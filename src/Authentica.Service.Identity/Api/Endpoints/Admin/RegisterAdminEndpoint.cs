using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Domain.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Admin;

/// <summary>
/// Endpoint for registering a new admin user.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class RegisterAdminEndpoint : EndpointBaseAsync
                                     .WithRequest<RegisterRequest>
                                     .WithActionResult
{
    /// <summary>
    /// Gets the service provider for resolving dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterAdminEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider for resolving dependencies.</param>
    public RegisterAdminEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the HTTP POST request for registering a new admin user.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Admin.Create}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleDefaults.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(RegisterRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        var publisher = Services.GetRequiredService<IPublisher>();

        var result = await userWriteStore.CreateUserAsync(request, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        if (!await userManager.IsInRoleAsync(result.User, RoleDefaults.Admin))
        {
            await userManager.AddToRoleAsync(result.User, RoleDefaults.Admin);
            await userManager.AddToRoleAsync(result.User, RoleDefaults.User);
        }

        RegisterAdminActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        AdminUserRegistered @event = new(request.Email,
                                      DateTime.UtcNow,
                                      User.Identity?.Name ?? "Unknown");
        
        await publisher.PublishAsync(@event, cancellationToken);

        // call token endpoint for a email confirmation token.
        return StatusCode(StatusCodes.Status201Created);
    }
}