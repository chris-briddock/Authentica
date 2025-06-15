using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UserUpdateEmailCodeSent events.
/// </summary>
public class UserUpdateEmailCodeSentConsumer : IEmailConsumer<UserUpdateEmailCodeSent>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UserUpdateEmailCodeSentConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserUpdateEmailCodeSentConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UserUpdateEmailCodeSentConsumer(
        EmailService emailService,
        ILogger<UserUpdateEmailCodeSentConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UserUpdateEmailCodeSent event and sends an email update code.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserUpdateEmailCodeSent> context)
    {
        try
        {
            _logger.LogInformation("Consuming UserUpdateEmailCodeSent event for {Email}", context.Message.Email);
            
            var subject = "Update Your Email Address";
            var emailBody = _emailService.CreateEmailTemplate(
                "Update Email Address",
                "Update your email address",
                context.Message.Email,
                $"Your email update code is <span class=\"font-bold text-indigo-800\">{context.Message.Code}</span>",
                "Please use this code to verify your new email address."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Email update code sent successfully to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UserUpdateEmailCodeSent event for {Email}", context.Message.Email);
            throw;
        }
    }
}
