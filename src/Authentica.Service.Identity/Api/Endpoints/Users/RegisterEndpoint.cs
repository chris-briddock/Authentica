using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Exposes an endpoint that allows a user to register.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class RegisterEndpoint : EndpointBaseAsync
                                       .WithRequest<RegisterRequest>
                                       .WithActionResult
{
    /// <summary>
    /// The application's service provider.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="RegisterEndpoint"/>
    /// </summary> 
    public RegisterEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP POST request for creating a new user.
    /// </summary>
    /// <param name="request">The authentication request containing the email and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// <see cref="ActionResult"/> indicating the result of the authentication attempt.
    /// Returns <see cref="StatusCodes.Status201Created"/> if the user is created..
    /// Returns <see cref="StatusCodes.Status409Conflict"/> if the user exists.
    /// Returns <see cref="StatusCodes.Status500InternalServerError"/> in case of an internal server error.
    /// </returns>
    [HttpPost($"{Routes.Users.Create}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(RegisterRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var userWriteStore = Services.GetRequiredService<IUserWriteStore>();

        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null && !existingUser.IsDeleted)
            return StatusCode(StatusCodes.Status409Conflict, "User is deleted, or already exists.");

        var result = await userWriteStore.CreateUserAsync(request, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        if (!await userManager.IsInRoleAsync(result.User, RoleDefaults.User))
            await userManager.AddToRoleAsync(result.User, RoleDefaults.User);

        // Send confirmation email - trigger domain event.
        return StatusCode(StatusCodes.Status201Created);
    }
}
