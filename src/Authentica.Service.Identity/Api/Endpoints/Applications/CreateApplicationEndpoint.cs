using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Domain.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Applications;

/// <summary>
/// Endpoint for creating applications.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class CreateApplicationEndpoint : EndpointBaseAsync
                                                .WithRequest<CreateApplicationRequest>
                                                .WithActionResult
{
    /// <summary>
    /// Provides access to application services.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CreateApplicationEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public CreateApplicationEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    /// <summary>
    /// Creates a new application tied to a user.
    /// </summary>
    /// <param name="request">The object which encapsulates the request body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of the application being created.</returns>
    [HttpPost($"{Routes.Applications.Create}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync([FromBody] CreateApplicationRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var readStore = Services.GetRequiredService<IApplicationReadStore>();
        var writeStore = Services.GetRequiredService<IApplicationWriteStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        var publisher = Services.GetRequiredService<IPublisher>();

        var applicationExists = await readStore.CheckApplicationExistsByNameAsync(request.Name, cancellationToken);

        if (applicationExists)
            return BadRequest("Application with this name already exists.");

        var result = await writeStore.CreateClientApplicationAsync(User, request.Name, request.CallbackUri, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        CreatedApplicationActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        ApplicationCreated @event = new(request.Name,
                                        DateTime.UtcNow,
                                        User.Identity?.Name ?? "Unknown");

        await publisher.PublishAsync(@event, cancellationToken);

        return StatusCode(StatusCodes.Status201Created);
    }
}