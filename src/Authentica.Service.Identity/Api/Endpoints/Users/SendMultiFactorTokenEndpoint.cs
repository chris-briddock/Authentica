using Api.Constants;
using Ardalis.ApiEndpoints;
using Domain.Aggregates.Identity;
using Domain.Contracts;
using Domain.Events;
using Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for sending a multi-factor authentication token.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class SendMultiFactorTokenEndpoint : EndpointBaseAsync
                                            .WithRequest<SendMultiFactorTokenRequest>
                                            .WithActionResult
{
    /// <summary>
    /// Gets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SendMultiFactorTokenEndpoint"/>
    /// </summary>
    /// <param name="services"></param>
    public SendMultiFactorTokenEndpoint(IServiceProvider services)
    {
        Services = services;
    }
    /// <summary>
    /// Handles the POST request to send a multi-factor authentication token.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost($"{Routes.Users.Codes.SendMultiFactorToken}")]
    public override async Task<ActionResult> HandleAsync(SendMultiFactorTokenRequest request,
                                                         CancellationToken cancellationToken = default)
    {
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var publisher = Services.GetRequiredService<IPublisher>();

         User? user = await userManager.FindByEmailAsync(request.Email)!;

         if (user is null)
            return BadRequest();

        var token = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

        MfaEmailCodeSent @event = new(user.Email!, token, DateTime.UtcNow);
        
        await publisher.PublishAsync(@event, cancellationToken);

        return Ok();


    }
}

