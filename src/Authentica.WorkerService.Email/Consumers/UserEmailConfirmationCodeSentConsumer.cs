using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UserEmailConfirmationCodeSent events.
/// </summary>
public class UserEmailConfirmationCodeSentConsumer : IEmailConsumer<UserEmailConfirmationCodeSent>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UserEmailConfirmationCodeSentConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmailConfirmationCodeSentConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UserEmailConfirmationCodeSentConsumer(
        EmailService emailService,
        ILogger<UserEmailConfirmationCodeSentConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UserEmailConfirmationCodeSent event and sends a confirmation email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserEmailConfirmationCodeSent> context)
    {
        try
        {
            _logger.LogInformation("Consuming UserEmailConfirmationCodeSent event for {Email}", context.Message.Email);
            
            var subject = "Please confirm your email address";
            var emailBody = _emailService.CreateEmailTemplate(
                "Confirm Your Email",
                "Confirm your email",
                context.Message.Email,
                $"Your confirmation email code is <span class=\"font-bold text-indigo-800\">{context.Message.Code}</span>"
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Email confirmation code sent successfully to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserEmailConfirmationCodeSent event for {Email}", context.Message.Email);
            throw;
        }
    }
}
