using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Application.Contracts.Stores;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Sessions;

/// <summary>
/// Represents an endpoint for deleting a session.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class DeleteSessionEndpoint : EndpointBaseAsync
                                            .WithRequest<DeleteSessionRequest>
                                            .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    private IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSessionEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public DeleteSessionEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the delete session request asynchronously.
    /// </summary>
    /// <param name="request">The delete session request containing the session ID.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (optional).</param>
    /// <returns>An ActionResult indicating the result of the operation.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete($"{Routes.Sessions.Name}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public override async Task<ActionResult> HandleAsync(DeleteSessionRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        var sessionReadStore = Services.GetRequiredService<ISessionReadStore>();
        var sessionWriteStore = Services.GetRequiredService<ISessionWriteStore>();

        Session? session = await sessionReadStore.GetByIdAsync(request.SessionId);
        
        if (session is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await sessionWriteStore.DeleteAsync(session);
        return NoContent();
    }
}

