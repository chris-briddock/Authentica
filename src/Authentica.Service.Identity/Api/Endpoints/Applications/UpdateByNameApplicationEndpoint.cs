using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Application.DTOs;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Events;
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
    /// Provides access to application services.
    /// </summary>
    public IServiceProvider Services { get; }

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
    /// <returns></returns> 
    [HttpPut($"{Routes.Applications.UpdateByName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public override async Task<ActionResult> HandleAsync(UpdateApplicationByNameRequest request, CancellationToken cancellationToken = default)
    {
        var userWriteStore = Services.GetRequiredService<IUserReadStore>();
        var writeStore = Services.GetRequiredService<IApplicationWriteStore>();
        var readStore = Services.GetRequiredService<IApplicationReadStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        var userReadResult = await userWriteStore.GetUserByEmailAsync(User, cancellationToken);
        var user = userReadResult.User;

        UpdateApplicationByNameEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);
        
        if (user is null)
            return BadRequest();

        ClientApplication? app = await readStore.GetClientApplicationByNameAndUserIdAsync(request.CurrentName,
                                                                                          user.Id,
                                                                                          cancellationToken);

        if (app is null)
            return BadRequest();

        var dto = new ApplicationDTO<UpdateApplicationByNameRequest>()
        {
            Request = request,
            ClaimsPrincipal = User
        };

        var result = await writeStore.UpdateApplicationAsync(dto, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        return Ok();
    }
}