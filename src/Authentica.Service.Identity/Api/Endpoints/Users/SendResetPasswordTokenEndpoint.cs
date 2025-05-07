using Api.Constants;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Common.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for sending a password reset token.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class SendResetPasswordTokenEndpoint : EndpointBaseAsync
                                             .WithRequest<SendResetPasswordTokenRequest>
                                             .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SendResetPasswordTokenEndpoint"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SendResetPasswordTokenEndpoint(IServiceProvider services)
    {
        Services = services;
    }
    /// <summary>
    /// Handles the HTTP POST request to send a password reset token.
    /// </summary>
    /// <param name="request">The request containing the user's email address.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns><see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Users.Codes.SendPasswordResetToken}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(SendResetPasswordTokenRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var publisher = Services.GetRequiredService<IPublisher>();

        User? user = await userManager.FindByEmailAsync(request.Email)!;

         if (user is null)
            return BadRequest();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        UserPasswordResetRequested @event = new(user.Email!, token, DateTime.UtcNow);
        
        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();


    }
}

