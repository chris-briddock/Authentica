using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UserPasswordReset events.
/// </summary>
public class UserPasswordResetConsumer : IEmailConsumer<UserPasswordReset>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UserPasswordResetConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordResetConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UserPasswordResetConsumer(
        EmailService emailService,
        ILogger<UserPasswordResetConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UserPasswordReset event and sends a password reset confirmation email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserPasswordReset> context)
    {
        try
        {
            _logger.LogInformation("Consuming UserPasswordReset event for {Email}", context.Message.Email);
            
            var subject = "Password Reset Successful";
            var emailBody = _emailService.CreateEmailTemplate(
                "Password Reset Successful",
                "Password Reset Successful",
                context.Message.Email,
                "Your password has been successfully reset.",
                "If you did not initiate this password reset, please contact our support team immediately as your account may have been compromised."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Password reset confirmation email sent to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserPasswordReset event for {Email}", context.Message.Email);
            throw;
        }
    }
}
