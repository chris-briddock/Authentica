using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Application.DTOs;
using Ardalis.ApiEndpoints;
using Domain.Events;
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
    public IServiceProvider Services { get; }

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
    public override async Task<ActionResult> HandleAsync(CreateApplicationRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var readStore = Services.GetRequiredService<IApplicationReadStore>();
        var writeStore = Services.GetRequiredService<IApplicationWriteStore>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        var applicationExists = await readStore.CheckApplicationExistsAsync(request.Name, cancellationToken);

        if (applicationExists)
            return BadRequest("Application with this name already exists.");

        var dto = new ApplicationDTO<CreateApplicationRequest>()
        {
            Request = request,
            ClaimsPrincipal = User
        };

        var result = await writeStore.CreateClientApplicationAsync(dto, cancellationToken);

        if (result.Errors.Any())
            return StatusCode(StatusCodes.Status500InternalServerError, result.Errors.First().Description);

        CreatedApplicationEvent @event = new()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        return StatusCode(StatusCodes.Status201Created);
    }
}