using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UserEmailConfirmed events.
/// </summary>
public class UserEmailConfirmedConsumer : IEmailConsumer<UserEmailConfirmed>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UserEmailConfirmedConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmailConfirmedConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UserEmailConfirmedConsumer(
        EmailService emailService,
        ILogger<UserEmailConfirmedConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UserEmailConfirmed event and sends a confirmation success email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserEmailConfirmed> context)
    {
        try
        {
            _logger.LogInformation("Consuming UserEmailConfirmed event for {Email}", context.Message.Email);
            
            var subject = "Email Confirmation Successful";
            var emailBody = _emailService.CreateEmailTemplate(
                "Email Confirmed",
                "Email Confirmation Successful",
                context.Message.Email,
                "Your email address has been successfully confirmed. Thank you for verifying your account.",
                "You can now access all features of your Authentica account."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Email confirmation success notification sent to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserEmailConfirmed event for {Email}", context.Message.Email);
            throw;
        }
    }
}
