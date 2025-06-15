using Authentica.WorkerService.Email.Services;
using Common.Events;
using MassTransit;

namespace Authentica.WorkerService.Email.Consumers;

/// <summary>
/// Consumer for handling MfaDisabled events.
/// </summary>
public class MfaDisabledConsumer : IEmailConsumer<MfaDisabled>
{
    private readonly EmailService _emailService;
    private readonly ILogger<MfaDisabledConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MfaDisabledConsumer"/> class.
    /// </summary>
    /// <param name="emailService">The email service.</param>
    /// <param name="logger">The logger.</param>
    public MfaDisabledConsumer(
        EmailService emailService,
        ILogger<MfaDisabledConsumer> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Consumes the MfaDisabled event and sends an MFA disabled notification email.
    /// </summary>
    /// <param name="context">The consume context containing the event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<MfaDisabled> context)
    {
        try
        {
            _logger.LogInformation("Consuming MfaDisabled event for {Email}", context.Message.Email);
            
            var subject = "Multi-Factor Authentication Disabled";
            var emailBody = _emailService.CreateEmailTemplate(
                "MFA Disabled",
                "Multi-Factor Authentication Disabled",
                context.Message.Email,
                "Multi-factor authentication has been disabled for your account.",
                "If you did not request this change, please contact our support team immediately as your account may have been compromised."
            );

            await _emailService.SendEmailAsync(context.Message.Email, subject, emailBody);
            
            _logger.LogInformation("MFA disabled notification sent to {Email}", context.Message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing MfaDisabled event for {Email}", context.Message.Email);
            throw;
        }
    }
}
