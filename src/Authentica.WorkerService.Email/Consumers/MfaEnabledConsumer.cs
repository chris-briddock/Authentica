using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling MfaEnabled events.
/// </summary>
public class MfaEnabledConsumer : IEmailConsumer<MfaEnabled>
{
    private readonly EmailService _emailService;
    private readonly ILogger<MfaEnabledConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MfaEnabledConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public MfaEnabledConsumer(
        EmailService emailService,
        ILogger<MfaEnabledConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the MfaEnabled event and sends an MFA enabled notification email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<MfaEnabled> context)
    {
        try
        {
            _logger.LogInformation("Consuming MfaEnabled event for {Email}", context.Message.Email);
            
            var subject = "Multi-Factor Authentication Enabled";
            var emailBody = _emailService.CreateEmailTemplate(
                "MFA Enabled",
                "Multi-Factor Authentication Enabled",
                context.Message.Email,
                "Multi-factor authentication has been successfully enabled for your account.",
                "This additional security measure will help protect your account from unauthorized access. Remember to keep your recovery codes in a safe place."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("MFA enabled notification sent to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing MfaEnabled event for {Email}", context.Message.Email);
            throw;
        }
    }
}
