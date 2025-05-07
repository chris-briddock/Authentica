using Api.Constants;
using Application.Activities;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Domain.Contracts.Stores;
using Common.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Applications;

/// <summary>
/// Endpoint for updating the application name.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class UpdateByNameApplicationEndpoint : EndpointBaseAsync
                                                      .WithRequest<UpdateApplicationByNameRequest>
                                                      .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateByNameApplicationEndpoint"/>
    /// </summary>
    /// <param name="services">The application's service provider.</param>
    public UpdateByNameApplicationEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    /// <summary>
    /// Allows a user to update an application.
    /// </summary>
    /// <param name="request">The object which encapsulates the request body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An asyncronous <see cref="Task"/></returns> 
    [HttpPut($"{Routes.Applications.UpdateByName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync([FromBody] UpdateApplicationByNameRequest request, CancellationToken cancellationToken = default)
    {
        var userWriteStore = Services.GetRequiredService<IUserReadStore>();
        var writeStore = Services.GetRequiredService<IApplicationWriteStore>();
        var readStore = Services.GetRequiredService<IApplicationReadStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        var publisher = Services.GetRequiredService<IPublisher>();

        var user = (await userWriteStore.GetUserByEmailAsync(User, cancellationToken)).User;

        if (user is null)
            return BadRequest();

        ClientApplication? app = await readStore.GetClientApplicationByNameAndUserIdAsync(request.CurrentName,
                                                                                          user.Id,
                                                                                          cancellationToken);

        if (app is null)
            return BadRequest();

        var result = await writeStore.UpdateApplicationAsync(User, request.CurrentName, request.NewName, request.NewCallbackUri, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        UpdateApplicationByNameActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        ApplicationUpdated @event = new(request.CurrentName,
                                        request.NewName,
                                        request.NewCallbackUri,
                                        DateTime.UtcNow,
                                        User.Identity?.Name ?? "Unknown");
        
        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();
    }
}