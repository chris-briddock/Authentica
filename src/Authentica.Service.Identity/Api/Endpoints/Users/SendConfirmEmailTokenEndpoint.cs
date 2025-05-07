using Api.Constants;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Common.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

/// <summary>
/// Endpoint for sending a confirmation email token.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class SendConfirmEmailTokenEndpoint : EndpointBaseAsync
                                             .WithRequest<SendConfirmEmailTokenRequest>
                                             .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SendConfirmEmailTokenEndpoint"/>
    /// </summary>
    /// <param name="services">The service provider used to resolve dependencies.</param>
    public SendConfirmEmailTokenEndpoint(IServiceProvider services)
    {
        Services = services;
    }
    /// <summary>
    /// Handles the POST request to send a confirm email token.
    /// </summary>
    /// <param name="request">The object which encapsulates the request</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ActionResult"/> </returns>
    [HttpPost($"{Routes.Users.Codes.SendEmailConfirmation}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(SendConfirmEmailTokenRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var publisher = Services.GetRequiredService<IPublisher>();

        User? user = await userManager.FindByEmailAsync(request.Email)!;

         if (user is null)
            return BadRequest();

        var code = await userManager.GenerateUserTokenAsync(user!, TokenOptions.DefaultEmailProvider, EmailTokenConstants.ConfirmEmail);

        UserEmailConfirmationCodeSent @event = new(user.Email!, code, DateTime.UtcNow);
        
        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();
    }
}


