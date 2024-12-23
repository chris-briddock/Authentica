using Api.Constants;
using Api.Requests;
using Application.Activities;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for user authentication.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class LoginEndpoint : EndpointBaseAsync
                                    .WithRequest<LoginRequest>
                                    .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to access application services.</param>
    public LoginEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Handles the HTTP POST request for user authentication.
    /// </summary>
    /// <param name="request">The authentication request containing the email and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// <see cref="ActionResult"/> indicating the result of the authentication attempt.
    /// Returns <see cref="StatusCodes.Status200OK"/> if the user is authenticated and does not require mfa.
    /// Returns <see cref="StatusCodes.Status401Unauthorized"/> if the authentication fails or requires mfa.
    /// </returns>
    [HttpPost($"{Routes.Users.Login}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(LoginRequest request,
                                                        CancellationToken cancellationToken = default)
    {
        var signInManager = Services.GetRequiredService<SignInManager<User>>();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var activityWriteStore = Services.GetRequiredService<IActivityWriteStore>();

        // Set the authentication scheme
        signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

        User? user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return BadRequest();

        // Attempt to sign in the user
        var signInResult = await signInManager.PasswordSignInAsync(request.Email, request.Password, true, true);

        LoginActivity activity = new()
        {
            Payload = request
        };

        await activityWriteStore.SaveActivityAsync(activity);

        // Check if the user requires mfa authentication
        if (signInResult.RequiresTwoFactor)
            return Ok("User requires mfa authentication.");

        // Check if the sign-in attempt was successful
        if (!signInResult.Succeeded)
            return Unauthorized();

        return Ok();
    }
}