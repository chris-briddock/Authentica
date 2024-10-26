using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Application.DTOs;
using Ardalis.ApiEndpoints;
using Application.Activities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Applications;

/// <summary>
/// Endpoint for creating application secrets.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class CreateApplicationSecretEndpoint : EndpointBaseAsync
                                               .WithRequest<CreateApplicationSecretRequest>
                                               .WithActionResult
{
     /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CreateApplicationSecretEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public CreateApplicationSecretEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Creates a new application secret.
    /// </summary>
    /// <param name="request">The object which encapsulates the request body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the application secret being created.</returns>
    [HttpPut($"{Routes.Applications.ApplicationSecrets}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(CreateApplicationSecretRequest request, CancellationToken cancellationToken = default)
    {
        var appReadStore = Services.GetRequiredService<IApplicationReadStore>();
        var appWriteStore = Services.GetRequiredService<IApplicationWriteStore>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();

        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);


        var app = await appReadStore.GetClientApplicationByNameAndUserIdAsync(request.Name,
                                                                              userReadResult.User!.Id,
                                                                              cancellationToken);
        if (app is null)
            return BadRequest();

        ApplicationDto<CreateApplicationSecretRequest> dto = new()
        {
            Request = request,
            ClaimsPrincipal = User
        };

        var result = await appWriteStore.UpdateClientSecretAsync(dto, cancellationToken);

       if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        CreatedApplicationSecretActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        return Ok(new { result.Secret });

    }
}