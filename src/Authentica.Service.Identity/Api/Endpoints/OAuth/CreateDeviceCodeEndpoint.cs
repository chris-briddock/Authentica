using Api.Constants;
using Application.Activities;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.OAuth;

/// <summary>
/// Endpoint for generating a device code as part of the Device Authorization Flow.
/// </summary>
/// <remarks>
/// This endpoint is called when a user on their mobile device initiates the Device Authorization Flow.
/// It generates a unique code tied to the user's account, which can be used in the authorization process.
/// </remarks>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class CreateDeviceCodeEndpoint : EndpointBaseAsync
                                               .WithoutRequest
                                               .WithActionResult
{
    /// <summary>
    /// Provides access to application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDeviceCodeEndpoint"/> class.
    /// </summary>
    /// <param name="services">The IServiceProvider for dependency injection.</param>
    public CreateDeviceCodeEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the HTTP GET request to generate a device code.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// An ActionResult containing the generated device code if successful, 
    /// or an appropriate error response if the operation fails.
    /// </returns>
    [HttpGet($"{Routes.OAuth.Device}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        // Get required services
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        // Retrieve the user based on their email
        var result = await userReadStore.GetUserByEmailAsync(User, cancellationToken);

        // Generate a unique device code for the user
        var code = await userManager.GenerateUserTokenAsync(result.User, TokenOptions.DefaultEmailProvider, TokenConstants.DeviceCode);

        CreateDeviceCodeActivity activity = new()
        {
            Payload = User.Identity?.Name ?? "Unknown"
        };

        await activityStore.SaveActivityAsync(activity);

        // Return the generated code
        return Ok(code);
    }
}
