using Api.Constants;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Common.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Common.Constants;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for sending a token for updating the email.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public sealed class SendUpdateEmailTokenEndpoint : EndpointBaseAsync
                                                   .WithRequest<SendUpdateEmailTokenRequest>
                                                   .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SendUpdateEmailTokenEndpoint"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SendUpdateEmailTokenEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the HTTP POST request to send a token for updating the email.
    /// </summary>
    /// <param name="request">The request containing the user's email address.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns><see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpPost($"{Routes.Users.Codes.SendEmailUpdateToken}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(SendUpdateEmailTokenRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var publisher = Services.GetRequiredService<IPublisher>();

        User? user = await userManager.FindByEmailAsync(request.Email)!;

         if (user is null)
            return BadRequest();

         var token = await userManager.GenerateUserTokenAsync(user!, TokenOptions.DefaultEmailProvider, EmailTokenConstants.UpdateEmail);

        UserUpdateEmailCodeSent @event = new(user.Email!, token, DateTime.UtcNow);
        
        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();
    }
}