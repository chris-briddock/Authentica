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
/// Represents an endpoint to delete an applicaion by name.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class DeleteByNameApplicationEndpoint : EndpointBaseAsync
                                               .WithRequest<DeleteApplicationByNameRequest>
                                               .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteByNameApplicationEndpoint"/>
    /// </summary>
    /// <param name="services"></param>
    public DeleteByNameApplicationEndpoint(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }
    /// <summary>
    /// Allows a user to soft delete an application.
    /// </summary>
    /// <param name="request">The object which encapsulates the request.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>The result of an application being soft deleted.</returns>
    [HttpDelete($"{Routes.Applications.DeleteByName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(DeleteApplicationByNameRequest request,
                                                        CancellationToken cancellationToken = default)
    {
        var readStore = Services.GetRequiredService<IApplicationReadStore>();
        var userReadStore = Services.GetRequiredService<IUserReadStore>();
        var userReadResult = await userReadStore.GetUserByEmailAsync(User, cancellationToken);
        var writeStore = Services.GetRequiredService<IApplicationWriteStore>();
        var activityStore = Services.GetRequiredService<IActivityWriteStore>();
        var publisher = Services.GetRequiredService<IPublisher>();

        var app = await readStore.GetClientApplicationByNameAndUserIdAsync(request.Name,
                                                                           userReadResult.User!.Id,
                                                                           cancellationToken);

        if (app is null)
            return BadRequest();

        var result = await writeStore.SoftDeleteApplicationAsync(User, app.Name, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        DeleteApplicationActivity activity = new()
        {
            Payload = request
        };

        await activityStore.SaveActivityAsync(activity);

        ApplicationDeleted @event = new(request.Name,
                                        DateTime.UtcNow,
                                        User.Identity?.Name ?? "Unknown");
        
        await publisher.PublishAsync(@event, cancellationToken);

        return NoContent();
    }
}
