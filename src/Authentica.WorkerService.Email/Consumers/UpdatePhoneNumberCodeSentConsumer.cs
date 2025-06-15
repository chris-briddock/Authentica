using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling UpdatePhoneNumberCodeSent events.
/// </summary>
public class UpdatePhoneNumberCodeSentConsumer : IEmailConsumer<UpdatePhoneNumberCodeSent>
{
    private readonly EmailService _emailService;
    private readonly ILogger<UpdatePhoneNumberCodeSentConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePhoneNumberCodeSentConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public UpdatePhoneNumberCodeSentConsumer(
        EmailService emailService,
        ILogger<UpdatePhoneNumberCodeSentConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the UpdatePhoneNumberCodeSent event and sends a phone number update code email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UpdatePhoneNumberCodeSent> context)
    {
        try
        {
            _logger.LogInformation("Consuming UpdatePhoneNumberCodeSent event for {Email}", context.Message.Email);
            
            var subject = "Update Your Phone Number";
            var emailBody = _emailService.CreateEmailTemplate(
                "Update Phone Number",
                "Update your phone number",
                context.Message.Email,
                $"Your phone number update code is <span class=\"font-bold text-indigo-800\">{context.Message.Code}</span>",
                "Please use this code to verify your new phone number."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("Phone number update code sent successfully to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing UpdatePhoneNumberCodeSent event for {Email}", context.Message.Email);
            throw;
        }
    }
}
