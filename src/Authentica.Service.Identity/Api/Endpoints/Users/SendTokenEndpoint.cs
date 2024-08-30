using Api.Constants;
using Api.Requests;
using Application.Contracts;
using Ardalis.ApiEndpoints;
using Authentica.Common;
using Domain.Aggregates.Identity;
using Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Users;

/// <summary>
/// Endpoint for sending a token based on the type of request made, such as two-factor authentication, email confirmation, 
/// password reset, or updating email/phone number. This endpoint handles the request, generates the appropriate token, 
/// and publishes it via email if the user exists.
/// </summary>
[Route($"{Routes.BaseRoute.Name}")]
public class SendTokenEndpoint : EndpointBaseAsync
                                .WithRequest<SendTokenRequest>
                                .WithActionResult
{

    /// <summary>
    /// Gets or sets the service provider used to resolve dependencies.
    /// </summary>
    public IServiceProvider Services { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendTokenEndpoint"/> class.
    /// </summary>
    /// <param name="services">The service provider used for dependency injection.</param>
    public SendTokenEndpoint(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Handles the HTTP POST request to send a token based on the specified token type. The token type determines whether
    /// to generate a two-factor authentication code, email confirmation code, password reset token, or a code for updating
    /// email or phone number. If the user exists, the token will be sent via email.
    /// </summary>
    /// <param name="request">The request containing the email address and token type.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> indicating the result of the operation.
    /// Returns <see cref="StatusCodes.Status200OK"/> with a success message if the email is sent.
    /// Returns <see cref="StatusCodes.Status400BadRequest"/> if the user does not exist.
    /// </returns>
    [HttpPost($"{Routes.Users.Tokens}")]
    [AllowAnonymous]
    public override async Task<ActionResult> HandleAsync(SendTokenRequest request,
                                                   CancellationToken cancellationToken = default)
    {
        EmailMessage message = new();
        var userManager = Services.GetRequiredService<UserManager<User>>();
        var emailPublisher = Services.GetRequiredService<IEmailPublisher>();
        var eventStore = Services.GetRequiredService<IEventStore>();

        User? user = await userManager.FindByEmailAsync(request.Email)!;

        var @event = new SendTokenIntegrationEvent()
        {
            Payload = request
        };

        await eventStore.SaveEventAsync(@event);

        switch (request.TokenType)
        {
            case EmailTokenConstants.TwoFactor:
                var twoFactorCode = await userManager.GenerateTwoFactorTokenAsync(user!, TokenOptions.DefaultEmailProvider);
                message = new()
                {
                    EmailAddress = user!.Email!,
                    Code = twoFactorCode,
                    Type = EmailTokenConstants.TwoFactor
                };
                break;

            case EmailTokenConstants.ConfirmEmail:
                var emailConfirmationCode = await userManager.GenerateUserTokenAsync(user!, TokenOptions.DefaultEmailProvider, EmailTokenConstants.ConfirmEmail);
                message = new()
                {
                    EmailAddress = user!.Email!,
                    Code = emailConfirmationCode,
                    Type = EmailTokenConstants.ConfirmEmail
                };
                break;

            case EmailTokenConstants.ResetPassword:
                var passwordResetCode = await userManager.GenerateUserTokenAsync(user!, TokenOptions.DefaultEmailProvider, EmailTokenConstants.ResetPassword);
                message = new()
                {
                    EmailAddress = user!.Email!,
                    Code = passwordResetCode,
                    Type = EmailTokenConstants.ResetPassword
                };
                break;

            case EmailTokenConstants.UpdateEmail:
                var updateEmailCode = await userManager.GenerateUserTokenAsync(user!, TokenOptions.DefaultEmailProvider, EmailTokenConstants.UpdateEmail);
                message = new()
                {
                    EmailAddress = user!.Email!,
                    Code = updateEmailCode,
                    Type = EmailTokenConstants.UpdateEmail
                };
                break;

            case EmailTokenConstants.UpdatePhoneNumber:
                var updatePhoneNumberCode = await userManager.GenerateUserTokenAsync(user!, TokenOptions.DefaultEmailProvider, EmailTokenConstants.UpdatePhoneNumber);
                message = new()
                {
                    EmailAddress = user!.Email!,
                    Code = updatePhoneNumberCode,
                    Type = EmailTokenConstants.UpdatePhoneNumber
                };
                break;
        }

        await emailPublisher.Publish(message, cancellationToken);
        return Ok($"If a user exists in the database, an email will be sent to that email address.");
    }
}
