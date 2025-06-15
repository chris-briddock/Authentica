using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UserPasswordResetRequested events.
/// </summary>
public class UserPasswordResetRequestedConsumer : IEmailConsumer<UserPasswordResetRequested>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UserPasswordResetRequestedConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordResetRequestedConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UserPasswordResetRequestedConsumer(
        EmailService emailService,
        ILogger<UserPasswordResetRequestedConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UserPasswordResetRequested event and sends a password reset email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserPasswordResetRequested> context)
    {
        try
        {
            _logger.LogInformation("Consuming UserPasswordResetRequested event for {Email}", context.Message.Email);
            
            var subject = "Password Reset Request";
            var emailBody = _emailService.CreateEmailTemplate(
                "Password Reset",
                "Forgotten your password?",
                context.Message.Email,
                $"Your password reset code is <span class=\"font-bold text-indigo-800\">{context.Message.Code}</span>",
                "This code will expire shortly. Please use it immediately to reset your password."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Password reset code sent successfully to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserPasswordResetRequested event for {Email}", context.Message.Email);
            throw;
        }
    }
}
